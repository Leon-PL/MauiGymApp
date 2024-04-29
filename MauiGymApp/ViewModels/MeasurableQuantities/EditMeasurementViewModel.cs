using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGymApp.Calculations;
using MauiGymApp.Models;
using MauiGymApp.Models.DTOs.Measurables;
using MauiGymApp.Services.Measurables;
using MauiGymApp.Services.Settings;
using MauiGymApp.State;
using MauiGymApp.ViewModels.Common;
using UnitsNet;

namespace MauiGymApp.ViewModels.MeasurableQuantities
{
     public partial class EditMeasurementViewModel : BaseViewModel
    {
        private readonly IMeasurableStateService _stateService;
        private readonly ISettingsService _settingsService;

        public EditMeasurementViewModel(IMeasurableStateService stateService, ISettingsService settingsService)
        {
            _stateService = stateService;
            _settingsService = settingsService;

            var query = (List<object>)_stateService.GetQueryModel<EditMeasurementViewModel>();
            MeasurableQuantity = (MeasurableQuantityViewModel)query[0];
            Measurement = (MeasurementViewModel)query[1];

            Value = Measurement.Value.As(_settingsService.GetUnitPreference(Measurement.QuantityType));
            Date = Measurement.DateTime;
            Image = Measurement.Image;
        }

        [ObservableProperty]
        MeasurableQuantityViewModel measurableQuantity;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MeasurementUnitString))]
        MeasurementViewModel measurement;

        [ObservableProperty]
        double value;

        [ObservableProperty]
        DateTime date;

        [ObservableProperty]
        byte[] image = [];

        public Enum? MeasurementUnit => Measurement is not null ?
            _settingsService.GetUnitPreference(Measurement.QuantityType) : null;

        public string MeasurementUnitString
        {
            get
            {
                if (Measurement?.QuantityType == QuantityType.Percentage) return "%";
                return MeasurementUnit is not null ?
                    UnitsNetHelpers.GetUnitAbreviation(MeasurementUnit) : "";
            }
        }

        [RelayCommand]
        async Task AddImage()
        {
            string action = await Shell.Current.DisplayActionSheet("Action", null, null, buttons: new string[] { "Take Photo", "From Gallery" });

            if (action == "Take Photo" && MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    using Stream sourceStream = await photo.OpenReadAsync();
                    using MemoryStream ms = new();

                    await sourceStream.CopyToAsync(ms);

                    Image = ms.ToArray();
                    return;
                }
            }

            if (action == "From Gallery")
            {
                var photo = await MediaPicker.Default.PickPhotoAsync();


                if (photo != null)
                {
                    using Stream stream = await photo.OpenReadAsync();

                    using MemoryStream ms = new();

                    await stream.CopyToAsync(ms);

                    Image = ms.ToArray();
                    return;
                }
                
            }

            await DisplayGenericErrorPrompt();
        }

        [RelayCommand]
        async Task DeleteMeasurement()
        {
            bool confirm = await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete", "Confirm", "Cancel");
            if (confirm)
            {
                MeasurableQuantity.Measurements.Remove(Measurement);
                await _stateService.UpdateMeasurableQuantity(MeasurableQuantity);
                await CloseAsync();
            }
        }

        [RelayCommand]
        async Task ConfirmAsync()
        {
            Measurement.Value = Quantity.From(Value, _settingsService.GetUnitPreference(Measurement.QuantityType));
            Measurement.Image = Image;
            Measurement.DateTime = Date;

            await _stateService.UpdateMeasurableQuantity(MeasurableQuantity);
            await CloseAsync();
        }

        [RelayCommand]
        async static Task CloseAsync() => await Shell.Current.GoToAsync("..", animate: true);
    }
}
