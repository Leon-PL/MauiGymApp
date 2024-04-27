using UnitsNet.Units;
using UnitsNet;
using MauiGymApp.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MauiGymApp.Services.Settings
{
    public interface ISettingsService
    {
        event Action MassUnitChanged;
        event Action LengthUnitChanged;
        event Action EquationsInUseChanged;

        bool UseEpley { get; set; }
        bool UseBryzcki { get; set; }
        bool UseLombardi { get; set; }
        bool UseMayhew { get; set; }
        bool UseOConnor { get; set; }
        bool UseWathan { get; set; }

        Enum GetUnitPreference(IQuantity quantity);
        Enum GetUnitPreference(QuantityType quantityType);
        double IQuantityAsPreferredUnit(IQuantity quantity);
        IQuantity IQuantityFromPreferredUnit(double value, QuantityType type);

        MassUnit MassUnit { get; set; }
        LengthUnit LengthUnit { get; set; }

        /// <summary>
        /// Maps quantity type to settings service property
        /// </summary>
        /// <returns></returns>
        IDictionary<QuantityType, Func<Enum>> QuantityTypeUnitsNetUnit { get; }
    }
}
