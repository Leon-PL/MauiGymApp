using MauiGymApp.Models;
using MauiGymApp.Models.DTOs.Measurables;
using MauiGymApp.Nito;
using MauiGymApp.Services.Measurables;
using MauiGymApp.Services.Settings;
using MauiGymApp.ViewModels.MeasurableQuantities;
using MauiGymApp.ViewModels.Utilities;
using MauiGymApp.Views.MeasurableQuantities;

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

        /// <summary>
        /// Dictionary for passing objects through navigation
        /// </summary>
        private readonly Dictionary<string, object> _queryModels = [];
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

        public object GetQueryModel<T>() => _queryModels[typeof(T).Name];


        public List<MeasurableQuantityViewModel> MeasurableQuantities { get; private set; } = [];

        #region CRUD
        public async Task AddMeasurableQuantity(MeasurableQuantityViewModel measurableQuantity)
        {
            await _measurablesService.AddAsync(measurableQuantity.ToModel());
            MeasurableQuantities.Add(measurableQuantity);
            MeasurableQuantitiesChanged?.Invoke();
        }

        public async Task AddMeasurableQuantities(IEnumerable<MeasurableQuantityViewModel> quantities)
        {
            await _measurablesService.AddRangeAsync(quantities.ToModels());
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
            var mq = (MeasurableQuantityViewModel)_queryModels[nameof(AddMeasurementViewModel)];
            mq.Measurements.Add(measurement);
            await _measurablesService.UpdateAsync(mq.ToModel());

             MeasurableQuantitiesChanged?.Invoke();
        }

        public async Task UpdateMeasurementAsync(MeasurementViewModel measurement)
        {
            var mq = (MeasurableQuantityViewModel)_queryModels[nameof(EditMeasurementViewModel)];
            await _measurablesService.UpdateAsync(mq.ToModel());
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
            _queryModels[nameof(MeasurementsViewModel)] = measurableQuantity;
            await Shell.Current.GoToAsync($"{nameof(MeasurementsPage)}", animate: true); 
        }

        public async Task GoToAddMeasurableQuantityAsync() 
            => await Shell.Current.GoToAsync($"{nameof(AddMeasurableQuantityPage)}", animate: true);
        

        public async Task GoToEditMeasurableQuantityAsync(MeasurableQuantityViewModel vm)
        {
            _queryModels[nameof(EditMeasurableQuantityViewModel)] = vm;    
            await Shell.Current.GoToAsync($"{nameof(EditMeasurableQuantityPage)}", animate: true);
        }

        public async Task GoToAddMeasurementAsync(MeasurableQuantityViewModel vm)
        {
            _queryModels[nameof(AddMeasurementViewModel)] = vm; 
            await Shell.Current.GoToAsync($"{nameof(AddMeasurementPage)}", animate: true);
        }

        public async Task GoToEditMeasurementAsync(MeasurableQuantityViewModel measurableQuantity, MeasurementViewModel measurement)
        {
            _queryModels[nameof(EditMeasurementPage)] = new List<object>() { measurableQuantity, measurement};
            await Shell.Current.GoToAsync($"{nameof(EditMeasurementPage)}", animate: true);
        }

        #endregion
    }
}
