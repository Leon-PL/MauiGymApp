using LiveChartsCore.Defaults;
using MauiGymApp.Calculations;
using MauiGymApp.Models;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Interfaces;
using MauiGymApp.ViewModels.MeasurableQuantities;
using MauiGymApp.ViewModels.Workouts;
using MauiGymApp.ViewModels.Workouts.Lifts;
using PluralizeService.Core;
using System.Globalization;
using UnitsNet;

namespace MauiGymApp.ViewModels.Utilities
{
    public static class ViewModelHelper
    {
        public static IEnumerable<T> ToModels<T>(this IEnumerable<DTOViewModel<T>> models)
           => models.Select(x => x.ToModel());

        public static IEnumerable<T> ToImplementations<T>(this IEnumerable<ITemplate<T>> templates)
            => templates.Select(t => t.ToImplementation());

        public static IEnumerable<DateTimePoint> ToDateTimePoints<T>(this IEnumerable<T> values, Func<T, DateTime> dateProp,
           Func<T, double> getValue) => values.Select(v => new DateTimePoint(dateProp(v), getValue(v)));

        public static Enum BaseUnitFromQuantityType(QuantityType quantityType)
        {
            return quantityType switch
            {
                QuantityType.Dimensionless => Scalar.BaseUnit,
                QuantityType.Mass => Mass.BaseUnit,
                QuantityType.Length => Length.BaseUnit,
                QuantityType.Time => Duration.BaseUnit,
                QuantityType.Percentage => Scalar.BaseUnit,
                _ => throw new ArgumentException(null, nameof(quantityType)),
            };
        }
        //TODO: Need to check
        //TODO: Move to view model.
        public static RoutineViewModel GetLatest(this IEnumerable<RoutineViewModel> routines)
        {
            if (routines.All(r => !r.Workouts.Any())) return routines.Last();
            return routines.OrderBy(r => r.Workouts?.Max(w => w.DateTime)).Last();
        }

        public static LiftWorkoutViewModel GetLatest(this IEnumerable<LiftWorkoutViewModel> liftWorkouts)
            => liftWorkouts.OrderBy(lw => lw.DateTime).Last();

        public static IEnumerable<MeasurementDifferentialViewModel> ToMeasurementDifferentials(this IEnumerable<MeasurementViewModel> measurements, Enum? unit = null)
        {
            if (!measurements.Any()) return [];
            var ordered = measurements.OrderByDescending(m => m.DateTime).ToList();
            

            if(unit is null)
            return Enumerable.Range(0, ordered.Count - 1)
                  .Select(i => new MeasurementDifferentialViewModel(ordered[i], ordered[i].Value.Substract(ordered[i + 1].Value)))
                  .Append(new MeasurementDifferentialViewModel(ordered[ordered.Count - 1], null));

            ordered.ForEach(o => o.ValueAs(unit));
            return Enumerable.Range(0, ordered.Count - 1)
                  .Select(i => new MeasurementDifferentialViewModel(ordered[i], ordered[i].Value.Substract(ordered[i + 1].Value).AsUnit(unit)))
                  .Append(new MeasurementDifferentialViewModel(ordered[ordered.Count - 1], null));
        }

        public static string FormatAsString(this MovementPattern pattern)
        {
            if (pattern.ToString() == "NA") return "N/A";
            return PluralizationProvider.Pluralize(string.Concat(pattern.ToString().Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' '));
        }

        public static MovementPattern MovementPatternFromFormattedString(string pattern)
        {
            if (pattern == "N/A") return MovementPattern.NA;
            var info = new CultureInfo("en-US", false).TextInfo;
            return Enum.Parse<MovementPattern>(PluralizationProvider.Singularize(info.ToTitleCase(pattern)));
        }
    }
}