using CommunityToolkit.Mvvm.ComponentModel;
using MauiGymApp.Models.DTOs.Measurables;
using MauiGymApp.ViewModels.Interfaces;
using UnitsNet;

namespace MauiGymApp.ViewModels.MeasurableQuantities
{
    public class MeasurementDifferentialViewModel : MeasurementViewModel
    {
        public MeasurementDifferentialViewModel(MeasurementDTO measurement,
            IQuantity? differential) : base(measurement)
        {
            Differential = differential;
        }

        public IQuantity? Differential { get; }

        public bool HasDifferential => Differential is not null;

        public bool ShowPercentage => IsPercentageQuantity && HasDifferential;

        /// <summary>
        /// Shell navigation breaks otherwise
        /// </summary>
        /// <returns></returns>
        public MeasurementViewModel AsMeasurementViewModel()
            => new(ToModel());
    }
}
