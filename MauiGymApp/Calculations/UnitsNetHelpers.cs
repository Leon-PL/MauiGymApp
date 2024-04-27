using UnitsNet.Units;
using UnitsNet;
using MauiGymApp.Models;
using DevExpress.Utils.Filtering;

namespace MauiGymApp.Calculations
{
    public static class UnitsNetHelpers
    {
        public static Enum GetBaseUnit(this IQuantity quantity)
            => quantity.QuantityInfo.BaseUnitInfo.Value;

        public static double AsBaseUnit(this IQuantity quantity)
            => quantity.As(quantity.GetBaseUnit());

        public static IQuantity AddQuantities(IQuantity q1, IQuantity q2)
        {
            CheckSameBaseUnits(q1, q2);

            var baseUnit = q1.GetBaseUnit();
            var result = q1.AsBaseUnit() + q2.AsBaseUnit();

            return Quantity.From(result, unit: baseUnit);
        }

        public static IQuantity SubtractQuantities(IQuantity q1, IQuantity q2)
        {
            CheckSameBaseUnits(q1, q2);

            var baseUnit = q1.GetBaseUnit();
            var result = q1.AsBaseUnit() - q2.AsBaseUnit();

            return Quantity.From(result, unit: baseUnit);
        }

        public static IQuantity Substract(this IQuantity q1, IQuantity q2)
            => SubtractQuantities(q1, q2);

        public static void CheckSameBaseUnits(IQuantity q1, IQuantity q2)
        {
            if (q1.GetBaseUnit() != q2.GetBaseUnit())
                throw new ArgumentException("Quantities must be of same dimensions");
        }

        public static string GetUnitAbreviation(Enum unit)
        {
            return unit switch
            {
                MassUnit.Kilogram => "Kg",
                MassUnit.Pound => "Lbs",
                LengthUnit.Meter => "M",
                LengthUnit.Centimeter => "Cm",
                LengthUnit.Inch => "Inch",
                _ => throw new ArgumentException("Unit not matched", nameof(unit)),
            };
        }

        public static Dictionary<string, MassUnit> MassUnitPickerOptions = new()
        {
            { "Kilograms", MassUnit.Kilogram },
            { "Pounds", MassUnit.Pound },
        };

        public static Dictionary<string, LengthUnit> LengthUnitPickerOptions = new()
        {
            { "Metres", LengthUnit.Meter },
            { "Centimetres", LengthUnit.Centimeter },
            { "Inches", LengthUnit.Inch },
        };

        public static IQuantity AsUnit(this IQuantity quantity, Enum unit)
        => Quantity.From(quantity.As(unit), unit);

        public static Dictionary<TValue, TKey> Reverse<TKey, TValue>(this IDictionary<TKey, TValue> source) where TValue : notnull
        {
            var dictionary = new Dictionary<TValue, TKey>();
            foreach (var entry in source)
            {
                if (!dictionary.ContainsKey(entry.Value))
                    dictionary.Add(entry.Value, entry.Key);
            }
            return dictionary;
        }
    }
}
