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

        bool _measurementLoaded = false;

        public EditMeasurementViewModel(IMeasurableStateService stateService, ISettingsService settingsService)
        {
            _stateService = stateService;
            _settingsService = settingsService;

            MeasurableQuantity = _stateService.MeasurableQuantityQuery!;
            Measurement = _stateService.MeasurementQuery!;
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

        partial void OnMeasurementChanged(MeasurementViewModel value)
        {
            if (_measurementLoaded) return;
            
            Value = Measurement.Value.As(_settingsService.GetUnitPreference(Measurement.Value));
            Date = Measurement.DateTime;
            Image = Measurement.Image;
            
            _measurementLoaded = true;
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
                await _stateService.DeleteMeasurementAsync(Measurement);
                await CloseAsync();
            }
        }

        [RelayCommand]
        async Task ConfirmAsync()
        {
            if (Measurement is null)
                return;

            var measurement = new MeasurementDTO()
            {
                Id = Measurement.ToModel().Id,
                ValueSI = Quantity.From(Value, _settingsService.GetUnitPreference(Measurement.Value)).AsBaseUnit(),
                DateTime = Date,
                QuantityType = Measurement.QuantityType,
                Image = Image,
            };

            await _stateService.UpdateMeasurementAsync(new MeasurementViewModel(measurement));
            await Shell.Current.GoToAsync("..", animate: true);
        }

        [RelayCommand]
        async static Task CloseAsync() => await Shell.Current.GoToAsync("..");
    }
}
