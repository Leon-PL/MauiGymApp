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
    public partial class AddMeasurementViewModel : BaseViewModel
    {
        private readonly IMeasurableStateService _stateService;
        private readonly ISettingsService _settingsService;

        public AddMeasurementViewModel(IMeasurableStateService stateService, ISettingsService settingsService)
        {
            _stateService = stateService;
            _settingsService = settingsService;

            MeasurableQuantity = (MeasurableQuantityViewModel)_stateService.GetQueryModel<AddMeasurementViewModel>();
            Date = DateTime.Now;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsValidValue))]
        double value;

        [ObservableProperty]
        DateTime date;

        [ObservableProperty]
        byte[] image = [];
         
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MeasurementUnitString))]
        MeasurableQuantityViewModel measurableQuantity;

        public Enum? MeasurementUnit => MeasurableQuantity is not null ?
            _settingsService.GetUnitPreference(MeasurableQuantity.QuantityType) : null;

        public string MeasurementUnitString
        {
            get
            {
                if (MeasurableQuantity?.QuantityType == QuantityType.Percentage) return "%";
                return MeasurementUnit is not null ?
                     UnitsNetHelpers.GetUnitAbreviation(MeasurementUnit) : "";
            }
        }

        public bool IsValidValue => double.IsNormal(Value);

        public IQuantity Measurement => _settingsService.IQuantityFromPreferredUnit(Value, MeasurableQuantity.QuantityType);
           

        [RelayCommand]
        async Task ConfirmAsync()
        {
            if (!IsValidValue)
                return;

            MeasurementDTO measurement = new()
            {
                DateCreated = DateTime.Now,
                ValueSI = Measurement.AsBaseUnit(),
                QuantityType = MeasurableQuantity.QuantityType,
                DateTime = Date,
                Image = Image,
            };

            MeasurableQuantity.Measurements.Add(new MeasurementViewModel(measurement));
            await _stateService.UpdateMeasurableQuantity(MeasurableQuantity);
            await Shell.Current.GoToAsync("../", animate: true);
        }

        [RelayCommand]
        async static Task CancelAsync() => await Shell.Current.GoToAsync("..", animate: true);

        [RelayCommand]
        async Task AddImage()
        {
            string action = await Shell.Current.DisplayActionSheet("Action", null, null, buttons: new string[] { "Take Photo", "From Gallery" });

            if (action == "Take Photo" && MediaPicker.Default.IsCaptureSupported)
            {
                FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo is null)
                {
                    await DisplayGenericErrorPrompt("Could not retrive photo");
                    return;
                }
                
                using Stream sourceStream = await photo.OpenReadAsync();
                using MemoryStream ms = new();
                    
                await sourceStream.CopyToAsync(ms);

                Image = ms.ToArray();
            }

            if (action == "From Gallery")
            {
                FileResult? result = await MediaPicker.Default.PickPhotoAsync();
                if (result is null)
                {
                    await DisplayGenericErrorPrompt("Could not retrieve image");
                    return;
                }
                using Stream stream = await result.OpenReadAsync();

                using MemoryStream ms = new();

                await stream.CopyToAsync(ms);

                Image = ms.ToArray();
            }
        }
    }
}
