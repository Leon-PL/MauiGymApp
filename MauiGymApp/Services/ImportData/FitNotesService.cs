using MauiGymApp.Models.DTOs.Measurables;
using MauiGymApp.Data.Importing;
using MauiGymApp.Models.DTOs.WeightLifting;
using Microsoft.Data.Analysis;
using MauiGymApp.State;
using MauiGymApp.ViewModels.Workouts.Lifts;
using MauiGymApp.ViewModels.Workouts;
using MauiGymApp.ViewModels.MeasurableQuantities;
using MauiGymApp.ViewModels.Utilities;

namespace MauiGymApp.Services.ImportData
{
    public class FitNotesService : IImportDataService
    {
        private readonly IMeasurableStateService _measurableStateService;
        private readonly IWorkoutStateService _workoutStateService;
        private readonly ILiftsStateService _liftsStateService;

        public FitNotesService(IMeasurableStateService measurableStateService, IWorkoutStateService workoutService, ILiftsStateService liftService)
        {
            _measurableStateService = measurableStateService;
            _workoutStateService = workoutService;
            _liftsStateService = liftService;
        }

        public static DataFrame GetWorkoutDataFrame(string filePath)
        {
            var df = DataFrame.LoadCsv(filePath);

            df.Columns.Remove("Distance");
            df.Columns.Remove("Distance Unit");
            df.Columns.Remove("Time");

            return df;
        }

        public async Task<IEnumerable<MeasurableQuantityDTO>> ImportMeasurableQuantitiesAsync(string dataPath)
        {
            var quantities = await Task.Run(() => FitNotesBodyImports.ReadMeasurableQuantities(dataPath));
            await _measurableStateService.AddMeasurableQuantities(quantities.Select(q => new MeasurableQuantityViewModel(q)));
            return quantities;
        }

        public async Task<IEnumerable<WorkoutDTO>> ImportWorkoutsAsync(string dataPath)
        {
            var df = GetWorkoutDataFrame(dataPath);
            var existing = _liftsStateService.Lifts;
            var existingNames = existing.Select(e => e.Name);

            var liftsToAdd = GetUniqueLiftNames(df).Where(n => !existingNames.Contains(n)).Select(n => new LiftDTO() { 
                Name = n,
                MovementPattern = Models.MovementPattern.NA,
                DateCreated = DateTime.Now,
            });


            var lifts = _liftsStateService.Lifts.ToModels().ToList();
            lifts.AddRange(liftsToAdd);
      
            var days = GetDaysWithWorkouts(df);

            List<WorkoutDTO> workouts = [];

            foreach (var day in days)
            {
                List<LiftWorkoutDTO> liftWorkouts = [];
                var dayDF = df.Filter(df["Date"].ElementwiseEquals(day));

                var dayLifts = GetUniqueLiftNames(dayDF);

                foreach (var liftName in dayLifts)
                {
                    var liftDayDF = dayDF.Filter(dayDF["Exercise"].ElementwiseEquals(liftName));

                    var liftWorkout = new LiftWorkoutDTO()
                    {
                        Lift = lifts.First(l => l.Name == liftName),
                        DateTime = day,
                        DateCreated = DateTime.Now,
                    };

                    for (var i = 0; i < liftDayDF.Rows.Count; i++)
                    {
                        var set = new SetDTO
                        {
                            Reps = (int)liftDayDF["Reps"].Cast<float>().ToList()[i],
                            RIR = null,
                            WeightKg = liftDayDF["Weight"].Cast<float>().ToList()[i],
                            DateCreated = day,
                        };

                        liftWorkout.Sets.Add(set);
                    }

                    liftWorkouts.Add(liftWorkout);
                }

                var workout = new WorkoutDTO()
                {
                    Name = "",
                    LiftWorkouts = liftWorkouts,
                    DateTime = day,
                    DateCreated = DateTime.Now,
                    Notes = "",
                };
                workouts.Add(workout);
            }

            await _workoutStateService.AddWorkouts(workouts.Select(w => new WorkoutViewModel(w)));
            return workouts;
        }

        private static IEnumerable<DateTime> GetDaysWithWorkouts(DataFrame fitNotesDataFrame)
        {
            var groups = fitNotesDataFrame.GroupBy("Date").First();

            return groups["Date"].Cast<DateTime>().ToList();
        }

        private static IEnumerable<string> GetUniqueLiftNames(DataFrame fitNotesDataFrame)  
            =>fitNotesDataFrame["Exercise"].Cast<string>().Select(x => x).Distinct().ToList();

        private static IEnumerable<LiftDTO> GetLiftDTOs(DataFrame fitNotesDataFrame)
        {
            var names = GetUniqueLiftNames(fitNotesDataFrame);
            return names.Select(n => new LiftDTO()
            {
                Name = n,
                MovementPattern = Models.MovementPattern.NA,
                DateCreated = DateTime.Now
            });
        }
    }
}
