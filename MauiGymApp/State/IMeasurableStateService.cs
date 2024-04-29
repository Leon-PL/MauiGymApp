using MauiGymApp.ViewModels.MeasurableQuantities;

namespace MauiGymApp.State
{
    public interface IMeasurableStateService
    {
        public event Action? MeasurableQuantitiesChanged;
        public event Action? CurrentMeasurableQuantityChanged;

        public bool Loaded { get;  }
        public List<MeasurableQuantityViewModel> MeasurableQuantities { get; }

        public object GetQueryModel<T>();

        Task AddMeasurableQuantity(MeasurableQuantityViewModel quantity);
        Task AddMeasurableQuantities(IEnumerable<MeasurableQuantityViewModel> quantity);
        Task DeleteMeasurableQuantity(MeasurableQuantityViewModel quantity);
        Task UpdateMeasurableQuantity(MeasurableQuantityViewModel quantity);

        #region Navigation
        Task GoToAddMeasurableQuantityAsync();
        Task GoToEditMeasurableQuantityAsync(MeasurableQuantityViewModel measurableQuantity);

        Task GoToMeasurementsAsync(MeasurableQuantityViewModel measurableQuantity);
        Task GoToAddMeasurementAsync(MeasurableQuantityViewModel measurableQuantity);
        Task GoToEditMeasurementAsync(MeasurableQuantityViewModel measurableQuantity, MeasurementViewModel measurement);
        #endregion
    }
}
