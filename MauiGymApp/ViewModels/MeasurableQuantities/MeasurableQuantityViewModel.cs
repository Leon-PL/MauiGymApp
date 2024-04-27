using CommunityToolkit.Mvvm.ComponentModel;
using MauiGymApp.Models;
using MauiGymApp.Models.DTOs.Measurables;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Interfaces;
using MauiGymApp.ViewModels.Utilities;
using System.Collections.ObjectModel;

namespace MauiGymApp.ViewModels.MeasurableQuantities
{
    public partial class MeasurableQuantityViewModel : DTOViewModel<MeasurableQuantityDTO>, IShowUnits
    {
        private readonly MeasurableQuantityDTO _measurableQuantity;

        public MeasurableQuantityViewModel(MeasurableQuantityDTO quantity)
        {
            _measurableQuantity = quantity;
            Name = _measurableQuantity.Name;
            QuantityType = _measurableQuantity.QuantityType;
            Goal = _measurableQuantity.Goal != null ? new GoalViewModel(_measurableQuantity.Goal) : null;
            Measurements = new ObservableCollection<MeasurementViewModel>(_measurableQuantity.Measurements.Select(m => new MeasurementViewModel(m)));
            Notes = _measurableQuantity.Notes;
        }

        [ObservableProperty]
        string name;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsPercentageQuantity), nameof(IsNotPercentageQuantity))]
        QuantityType quantityType;

        [ObservableProperty]
        string notes;

        [ObservableProperty]
        GoalViewModel? goal;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MeasurementDifferentials), nameof(FirstMeasurement), nameof(LatestMeasurement), nameof(Goal))]
        ObservableCollection<MeasurementViewModel> measurements;

        public IEnumerable<MeasurementDifferentialViewModel> GetDifferentials(Enum unit) => Measurements.ToMeasurementDifferentials(unit);
        public IEnumerable<MeasurementDifferentialViewModel> MeasurementDifferentials => Measurements.ToMeasurementDifferentials();

        public bool IsPercentageQuantity => QuantityType == QuantityType.Percentage;
        public bool IsNotPercentageQuantity => !IsPercentageQuantity;

        public MeasurementViewModel? FirstMeasurement
        {
            get
            {
                if (Measurements.Count > 0)
                    return Measurements.OrderBy(m => m.DateTime).FirstOrDefault();
                return null;
            }
        }

        public MeasurementViewModel? LatestMeasurement
        {
            get
            {   
                if (Measurements.Count > 0) 
                    return Measurements.OrderByDescending(m => m.DateTime).FirstOrDefault();
                return null;
            }
        }

        public override MeasurableQuantityDTO ToModel()
        {
            _measurableQuantity.Name = Name;
            _measurableQuantity.QuantityType = QuantityType;
            _measurableQuantity.Goal = Goal?.ToModel();
            _measurableQuantity.Notes = Notes;

             return _measurableQuantity;
        }

        public void ValueAs(Enum unit)
        {
            foreach (var item in Measurements)
            {
                item.ValueAs(unit);
            }
        }  
    }
}
