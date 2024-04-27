//using DevExpress.Maui.Core.Internal;
//using MauiGymApp.Models.DTOs.WeightLifting;
//using Microsoft.Data.Analysis;
//using System.Diagnostics;

//namespace MauiGymApp.Data.Importing
//{
//    public static class FitNotesWorkoutImports
//    {
//        public static DataFrame GetFitNotesDataFrame(string filePath = @"C:\Users\leonp\source\repos\MauiGymApp\MauiGymApp\Data\Importing\FitNotes_Export.csv")
//        {
//            var df = DataFrame.LoadCsv(filePath);

//            df.Columns.Remove("Distance");
//            df.Columns.Remove("Distance Unit");
//            df.Columns.Remove("Time");

//            return df;
//        }

//        public static IEnumerable<string> GetLiftNamesFromFitNotes(DataFrame fitNotesDataFrame)
//            => fitNotesDataFrame["Exercise"].Cast<string>().Select(x => x).Distinct().ToList();

//        /// <summary>
//        /// Neccessary because EF core does not recognise that the lift objects are identical
//        /// </summary>
//        /// <param name="fitNotesDataFrame"></param>
//        /// <returns></returns>
//        public static IEnumerable<LiftDTO> GetLiftDTOs(DataFrame fitNotesDataFrame)
//        {
//            return GetLiftNamesFromFitNotes(fitNotesDataFrame).Select(n => new LiftDTO()
//            {
//                Name = n,
//                MovementPattern = Models.MovementPattern.NA,
//                DateCreated = DateTime.Now
//            });
//        }

//        public static IEnumerable<DateTime> GetDaysWithWorkouts(DataFrame fitNotesDataFrame)
//        {
//            var groups = fitNotesDataFrame.GroupBy("Date").First();

//            return groups["Date"].Cast<DateTime>().ToList();
//        }

//        public static IEnumerable<WorkoutDTO> GetWorkoutsDTOFromFitNotes(string filePath)
//        {   
//            var fitNotesDataFrame = DataFrame.LoadCsv(filePath);
//            var names = GetLiftNamesFromFitNotes(fitNotesDataFrame);
            

//            var days = GetDaysWithWorkouts(fitNotesDataFrame);
            
//            List<WorkoutDTO> workouts = [];

//            foreach (var day in days)
//            {
//                List<LiftWorkoutDTO> liftWorkouts = [];
//                var dayDF = fitNotesDataFrame.Filter(fitNotesDataFrame["Date"].ElementwiseEquals(day));

//                var dayLifts = GetLiftNamesFromFitNotes(dayDF);

//                foreach (var liftName in dayLifts)
//                {
//                    var liftDayDF = dayDF.Filter(dayDF["Exercise"].ElementwiseEquals(liftName));
  
//                    var liftWorkout = new LiftWorkoutDTO()
//                    {
//                        Lift = lifts.First(l => l.Name == liftName),
//                        DateTime = day,
//                        DateCreated = DateTime.Now,
//                    };

//                    for (var i = 0; i < liftDayDF.Rows.Count; i++)
//                    {
//                        var set = new SetDTO
//                        {
//                            Reps = 1,//liftDayDF["Reps"].Cast<int>().ToList()[i],
//                            RIR = null,
//                            WeightKg = liftDayDF["Weight"].Cast<float>().ToList()[i],
//                            DateCreated = day,
//                        };

//                        liftWorkout.Sets.Add(set);
//                    }

//                    liftWorkouts.Add(liftWorkout);
//                }

//                var workout = new WorkoutDTO()
//                {
//                    Name = "",
//                    LiftWorkouts = liftWorkouts,
//                    DateTime = day,
//                    DateCreated = DateTime.Now,
//                    Notes = "",

//                };

//                workouts.Add(workout);
//            }

            
//            return workouts;
//        }
//    }
//}
