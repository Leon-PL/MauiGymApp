using CommunityToolkit.Mvvm.ComponentModel;
using MauiGymApp.Calculations;
using MauiGymApp.Models;
using MauiGymApp.Models.DTOs.Measurables;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Interfaces;
using MauiGymApp.ViewModels.Utilities;
using UnitsNet;

namespace MauiGymApp.ViewModels.MeasurableQuantities
{
    public partial class MeasurementViewModel : DTOViewModel<MeasurementDTO>, IShowUnits
    {
        private readonly MeasurementDTO _measurement;

        public MeasurementViewModel(MeasurementDTO measurement)
        {
            _measurement = measurement;
            Value = Quantity.From(_measurement.ValueSI,
                ViewModelHelper.BaseUnitFromQuantityType(_measurement.QuantityType));
            DateTime = _measurement.DateTime;
            Image = _measurement.Image;
            QuantityType = _measurement.QuantityType;
        }

        [ObservableProperty]
        IQuantity value;

        [ObservableProperty]
        DateTime dateTime;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasImage))]
        byte[]? image;

        public QuantityType QuantityType { get; }

        public bool HasImage => Image is not null && Image.Length > 0;

        public bool IsPercentageQuantity => QuantityType == QuantityType.Percentage;

        public bool IsNotPercentageQuantity => !IsPercentageQuantity;

        /// <summary>
        /// Used so that changing unit causes OnPropertyChanged
        /// </summary>
        /// <param name="unit"></param>
        public void ValueAs(Enum unit) => Value = Value.AsUnit(unit);

        public override MeasurementDTO ToModel()
        {
            _measurement.ValueSI = Value.AsBaseUnit();
            _measurement.DateTime = DateTime;
            _measurement.Image = Image;

            return _measurement;
        }
    }
}
