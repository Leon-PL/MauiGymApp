using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.Services.Settings;
using UnitsNet.Units;
using MauiGymApp.State;
using System.Collections.ObjectModel;

namespace MauiGymApp.ViewModels.MeasurableQuantities
{
    public partial class MeasurableQuantitiesViewModel : BaseViewModel
    {
        private readonly IMeasurableStateService _stateService;
        private readonly ISettingsService _settingsService;

        public MeasurableQuantitiesViewModel(IMeasurableStateService stateService, ISettingsService settingsService)
        {
            _stateService = stateService;
            _settingsService = settingsService;

            UpdateMeasurableQuantities();
            _stateService.MeasurableQuantitiesChanged += UpdateMeasurableQuantities;
         }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(OrderedMeasurableQuantities))]
        public IEnumerable<MeasurableQuantityViewModel> measurableQuantities = [];

        public MassUnit MassUnit => _settingsService.MassUnit;

        public LengthUnit LengthUnit => _settingsService.LengthUnit;

        public IEnumerable<MeasurableQuantityViewModel> OrderedMeasurableQuantities
            => MeasurableQuantities.OrderBy(q => q.LatestMeasurement);

        void UpdateMeasurableQuantities()
        {   
            MeasurableQuantities = new ObservableCollection<MeasurableQuantityViewModel>(_stateService.MeasurableQuantities); // temporary workaround
            MeasurableQuantities.ToList().ForEach(mq => mq.ValueAs(_settingsService.GetUnitPreference(mq.QuantityType)));
        }

        [RelayCommand]
        async Task DeleteMeasurableQuantityAsync(MeasurableQuantityViewModel quantity)
        {
            if (quantity is null)
            {
                await DisplayGenericErrorPrompt();
                return;
            }
                
            else
            {
                bool confirm = await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete", "Confirm", "Cancel");
                if (confirm) await _stateService.DeleteMeasurableQuantity(quantity);
                
            }
        }

        [RelayCommand]
        public async Task GoToAddMeasurableQuantityAsync() => await _stateService.GoToAddMeasurableQuantityAsync(  );

        [RelayCommand]
        async Task GoToMeasurementsAsync(MeasurableQuantityViewModel measurableQuantity) => await _stateService.GoToMeasurementsAsync(measurableQuantity);
    
        [RelayCommand]
        async Task ExportMeasurableQuantitiesAsync() => await DisplayGenericErrorPrompt("Feature Not Implemented");

        public override void Dispose()
        {
            base.Dispose();
            _stateService.MeasurableQuantitiesChanged -= UpdateMeasurableQuantities;
        }
    }
}
