using LiveChartsCore.Defaults;
using MauiGymApp.Models;
using MauiGymApp.Services.Calculator;
using MauiGymApp.Services.Settings;
using MauiGymApp.ViewModels.Utilities;
using MauiGymApp.ViewModels.Workouts;
using MauiGymApp.ViewModels.Workouts.Lifts;
using UnitsNet;

namespace MauiGymApp.ViewModels
{
    public static class LiftGraphsHelper
    {
        public static Dictionary<string, GraphType> GraphTypePickerOptions = new()
        {
            { "Estimated One Rep Max", GraphType.E1RM },
            { "Achieved One Rep Max", GraphType.Achieved1RM },
            { "Volume", GraphType.Volume }
        };

        public static IEnumerable<DateTimePoint>? ValuesFromGraphType(IEnumerable<LiftWorkoutViewModel>? liftWorkouts,
            GraphType graphType, ICalculatorService calculatorService, ISettingsService settingsService)
        {
            if (liftWorkouts is null) return null;

            Func<LiftWorkoutViewModel, double> valueFunc = graphType switch
            {
                GraphType.E1RM =>
                    (lw) =>     
                    {
                        var best = GetBestE1RM(lw, calculatorService);
                        if (best is null) return double.NaN;
                        return settingsService.IQuantityAsPreferredUnit(best);
                    },

                GraphType.Achieved1RM =>
                    (lw) => 
                    {

                        var sets = liftWorkouts.GetSets()?.Where(s => s.Reps == 1);
                        if (sets is null) return double.NaN;
                        if (!sets.Any()) return double.NaN;
                        return settingsService.IQuantityAsPreferredUnit(sets.Select(s => s.Weight).Max());
                    },
                
                GraphType.Volume =>
                    (lw) => 
                    {
                        var volume = liftWorkouts.GetSets()?.Sum(s => (s.Weight * s.Reps).As(settingsService.MassUnit));
                        if (volume is null) return double.NaN;
                        return volume.Value;
                    },
                _ => throw new NotImplementedException(),
            };
            return liftWorkouts.ToDateTimePoints(lw => lw.DateTime, valueFunc);
        }

        public static Mass? GetBestE1RM(LiftWorkoutViewModel liftWorkout, ICalculatorService service)
            => liftWorkout?.Sets.Where(s => s.Reps !=0).Max(s => service.OneRepMax(s.Weight, s.Reps));

        public static IEnumerable<SetViewModel>? GetSets(this IEnumerable<LiftWorkoutViewModel> liftWorkouts)
            => liftWorkouts.Where(lw => lw.Sets.Any()).SelectMany(lw => lw.Sets);
    }
}