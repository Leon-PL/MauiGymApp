using MauiGymApp.Models;
using UnitsNet;
using UnitsNet.Units;

namespace MauiGymApp.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        public event Action? MassUnitChanged;
        public event Action? LengthUnitChanged;
        public event Action? EquationsInUseChanged;

        public IDictionary<QuantityType, Func<Enum>> QuantityTypeUnitsNetUnit { get; }

        public SettingsService()
        {
            QuantityTypeUnitsNetUnit = new Dictionary<QuantityType, Func<Enum>>()
            {
                {QuantityType.Mass, () => MassUnit},
                {QuantityType.Length, () => LengthUnit}
            };
        }

        public Enum GetUnitPreference(IQuantity quantity)
        {
            var unitType = quantity.QuantityInfo.UnitType;
            if (unitType.Equals(typeof(ScalarUnit))) return ScalarUnit.Amount;
            if (unitType.Equals(typeof(MassUnit))) return MassUnit;
            if (unitType.Equals(typeof(LengthUnit))) return LengthUnit;
            if (unitType.Equals(typeof(DurationUnit))) return DurationUnit.Second;

            throw new ArgumentException(null, nameof(unitType));
        }

        public Enum GetUnitPreference(QuantityType quantityType)
        {
            return quantityType switch
            {
                QuantityType.Dimensionless => ScalarUnit.Amount,
                QuantityType.Mass => MassUnit,
                QuantityType.Length => LengthUnit,
                QuantityType.Time => DurationUnit.Second,
                QuantityType.Percentage => ScalarUnit.Amount,
                _ => throw new ArgumentException(null, nameof(quantityType)),
            };
        }

        public double IQuantityAsPreferredUnit(IQuantity quantity)
            => quantity.As(GetUnitPreference(quantity));

        public IQuantity IQuantityFromPreferredUnit(double value, QuantityType type) => Quantity.From(value, GetUnitPreference(type));

        #region Units
        public MassUnit MassUnit
        {
            get => Enum.Parse<MassUnit>(Preferences.Get(nameof(MassUnit), MassUnit.Kilogram.ToString()) ?? MassUnit.Kilogram.ToString(), ignoreCase: true);
            set
            {
                Preferences.Set(nameof(MassUnit), value.ToString());
                MassUnitChanged?.Invoke();
            }
        }

        public LengthUnit LengthUnit
        {
            get => Enum.Parse<LengthUnit>(Preferences.Get(nameof(LengthUnit), LengthUnit.Meter.ToString()) ?? LengthUnit.Meter.ToString(), ignoreCase: true);
            set
            {
                Preferences.Set(nameof(LengthUnit), value.ToString());
                LengthUnitChanged?.Invoke();
            }
        }
        #endregion

        #region OneRepMaxFunctions
        public bool UseEpley
        {
            get => Preferences.Get(nameof(UseEpley), true);
            set
            {
                Preferences.Set(nameof(UseEpley), value);
                EquationsInUseChanged?.Invoke();
            }
        }

        public bool UseBryzcki
        {
            get => Preferences.Get(nameof(UseBryzcki), true);
            set
            {
                Preferences.Set(nameof(UseBryzcki), value);
                EquationsInUseChanged?.Invoke();
            }
        }

        public bool UseLombardi
        {
            get => Preferences.Get(nameof(UseLombardi), true);
            set
            {
                Preferences.Set(nameof(UseLombardi), value);
                EquationsInUseChanged?.Invoke();
            }
        }

        public bool UseMayhew
        {
            get => Preferences.Get(nameof(UseMayhew), true);
            set
            {
                Preferences.Set(nameof(UseMayhew), value);
                EquationsInUseChanged?.Invoke();
            }
        }

        public bool UseOConnor
        {
            get => Preferences.Get(nameof(UseOConnor), true);
            set
            {
                Preferences.Set(nameof(UseOConnor), value);
                EquationsInUseChanged?.Invoke();
            }
        }

        public bool UseWathan
        {
            get => Preferences.Get(nameof(UseWathan), true);
            set
            {
                Preferences.Set(nameof(UseWathan), value);
                EquationsInUseChanged?.Invoke();
            }
        }
        #endregion
    }
}
