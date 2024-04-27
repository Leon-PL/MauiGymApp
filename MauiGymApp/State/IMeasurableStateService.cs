using MauiGymApp.ViewModels.MeasurableQuantities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiGymApp.State
{
    public interface IMeasurableStateService
    {
        public event Action? MeasurableQuantitiesChanged;
        public event Action? CurrentMeasurableQuantityChanged;

        public bool Loaded { get;  }
        public List<MeasurableQuantityViewModel> MeasurableQuantities { get; }

        /// <summary>
        /// For navigation purposes
        /// </summary>
        public MeasurableQuantityViewModel? MeasurableQuantityQuery { get; set; }
        public MeasurementViewModel? MeasurementQuery { get; set; }

        Task AddMeasurableQuantity(MeasurableQuantityViewModel quantity);
        Task AddMeasurableQuantities(IEnumerable<MeasurableQuantityViewModel> quantity);
        Task DeleteMeasurableQuantity(MeasurableQuantityViewModel quantity);
        Task UpdateMeasurableQuantity(MeasurableQuantityViewModel quantity);

        Task AddMeasurementAsync(MeasurementViewModel measurement);
        Task UpdateMeasurementAsync(MeasurementViewModel measurement);
        Task DeleteMeasurementAsync(MeasurementViewModel measurement);

        #region Navigation
        Task GoToAddMeasurableQuantityAsync();
        Task GoToEditMeasurableQuantityAsync(MeasurableQuantityViewModel measurableQuantity);

        Task GoToMeasurementsAsync(MeasurableQuantityViewModel measurableQuantity);
        Task GoToAddMeasurementAsync(MeasurableQuantityViewModel measurableQuantity);
        Task GoToEditMeasurementAsync(MeasurableQuantityViewModel measurableQuantity, MeasurementViewModel measurement);
        #endregion
    }
}
