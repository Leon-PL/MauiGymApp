using CommunityToolkit.Mvvm.Input;
using MauiGymApp.Models;
using MauiGymApp.Models.DTOs.Measurables;
using MauiGymApp.Nito;
using MauiGymApp.Services.Measurables;
using MauiGymApp.Services.Settings;
using MauiGymApp.ViewModels.MeasurableQuantities;
using MauiGymApp.Views.MeasurableQuantities;
using System.ComponentModel;

namespace MauiGymApp.State
{
    /// <summary>
    /// Ground truth for measurable objects, notifies viewmodel of changes.
    /// View models should not hold states that can be changed from somewhere else, otherwise listening to changes is not trivial
    /// </summary>
    public class MeasurableStateService : IMeasurableStateService
    {
        private readonly IMeasurablesService _measurablesService;
        private readonly ISettingsService _settingsService;

        private IEnumerable<MeasurableQuantityDTO> _measurableQuantities { get; set; } = [];
        private readonly NotifyTask<IEnumerable<MeasurableQuantityViewModel>> LoadMeasurableQuantitiesTask;

        public event Action? MeasurableQuantitiesChanged;
        public event Action? CurrentMeasurableQuantityChanged;
        public bool Loaded => LoadMeasurableQuantitiesTask.IsSuccessfullyCompleted;

        public MeasurableStateService(IMeasurablesService measurablesService, ISettingsService settingsService)
        {
            _measurablesService = measurablesService;
            _settingsService = settingsService;

            _settingsService.MassUnitChanged += UpdateUnits;
            _settingsService.LengthUnitChanged += UpdateUnits;

            LoadMeasurableQuantitiesTask = NotifyTask.Create(Load, []);
        }


        public async Task<IEnumerable<MeasurableQuantityViewModel>> Load()
        {
            _measurableQuantities = await _measurablesService.GetAllAsync();
            MeasurableQuantities = _measurableQuantities.Select(mq => new MeasurableQuantityViewModel(mq)).ToList();
            UpdateUnits();
            MeasurableQuantitiesChanged?.Invoke();
            return MeasurableQuantities;
        }

        public List<MeasurableQuantityViewModel> MeasurableQuantities { get; private set; } = [];
        public MeasurableQuantityViewModel? MeasurableQuantityQuery { get; set; }
        public MeasurementViewModel? MeasurementQuery { get; set; }

        #region CRUD
        public async Task AddMeasurableQuantity(MeasurableQuantityViewModel measurableQuantity)
        {
            await _measurablesService.AddAsync(measurableQuantity.ToModel());
            MeasurableQuantities.Add(measurableQuantity);
            MeasurableQuantitiesChanged?.Invoke();
        }

        public async Task AddMeasurableQuantities(IEnumerable<MeasurableQuantityViewModel> quantities)
        {
            await _measurablesService.AddRangeAsync(quantities.Select(q => q.ToModel()));
            MeasurableQuantities.AddRange(quantities);
            MeasurableQuantitiesChanged?.Invoke();
        }

        public async Task DeleteMeasurableQuantity(MeasurableQuantityViewModel measurableQuantity)
        {
            await _measurablesService.DeleteAsync(measurableQuantity.ToModel());
            MeasurableQuantities.Remove(measurableQuantity);
            MeasurableQuantitiesChanged?.Invoke();
        }

        public async Task UpdateMeasurableQuantity(MeasurableQuantityViewModel measurableQuantity)
        {
            await _measurablesService.UpdateAsync(measurableQuantity.ToModel());
            MeasurableQuantitiesChanged?.Invoke();
        }

        public async Task AddMeasurementAsync(MeasurementViewModel measurement)
        {
            MeasurableQuantityQuery!.Measurements.Add(measurement);
            await _measurablesService.UpdateAsync(MeasurableQuantityQuery.ToModel());

             MeasurableQuantitiesChanged?.Invoke();
        }

        public async Task UpdateMeasurementAsync(MeasurementViewModel measurement)
        {
            await _measurablesService.UpdateAsync(MeasurableQuantityQuery!.ToModel());
            MeasurableQuantitiesChanged?.Invoke();
        }

        public async Task DeleteMeasurementAsync(MeasurementViewModel measurement)
        {

            MeasurableQuantityQuery!.Measurements.Remove(measurement);
            await _measurablesService.UpdateAsync(MeasurableQuantityQuery.ToModel());
            MeasurableQuantitiesChanged?.Invoke();
        }

        #endregion

        public void UpdateUnits()
        {
            foreach (var mq in MeasurableQuantities)
            {
                var qt = mq.QuantityType;
                if (qt == QuantityType.Mass) mq.ValueAs(_settingsService.MassUnit);
                else if (qt == QuantityType.Length) mq.ValueAs(_settingsService.LengthUnit);
            };
            MeasurableQuantitiesChanged?.Invoke();
        }

        #region Navigation
        public async Task GoToMeasurementsAsync(MeasurableQuantityViewModel measurableQuantity)
        {
            MeasurableQuantityQuery = measurableQuantity;
            await Shell.Current.GoToAsync($"{nameof(MeasurementsPage)}", animate: true); 
        }

        public async Task GoToAddMeasurableQuantityAsync() 
            => await Shell.Current.GoToAsync($"{nameof(AddMeasurableQuantityPage)}", animate: true);
        

        public async Task GoToEditMeasurableQuantityAsync(MeasurableQuantityViewModel vm)
        {
            MeasurableQuantityQuery = vm;
            await Shell.Current.GoToAsync($"{nameof(EditMeasurableQuantityPage)}", animate: true);
        }

        public async Task GoToAddMeasurementAsync(MeasurableQuantityViewModel vm)
        {
            MeasurableQuantityQuery = vm;
            await Shell.Current.GoToAsync($"{nameof(AddMeasurementPage)}", animate: true);
        }

        public async Task GoToEditMeasurementAsync(MeasurableQuantityViewModel measurableQuantity, MeasurementViewModel measurement)
        {
            MeasurableQuantityQuery = measurableQuantity;
            MeasurementQuery = measurement;
            await Shell.Current.GoToAsync($"{nameof(EditMeasurementPage)}", animate: true);
        }

        #endregion
    }
}
