using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Data.Extensions;
using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Utilities;
using MauiGymApp.ViewModels.Workouts.Lifts;
using System.Collections.ObjectModel;

namespace MauiGymApp.ViewModels.Workouts
{
    public partial class WorkoutViewModel : DTOViewModel<WorkoutDTO>
    {
        private readonly WorkoutDTO _workout;

        public WorkoutViewModel(WorkoutDTO workout)
        {
            _workout = workout;

            Name = _workout.Name;
            DateTime = _workout.DateTime;
            LiftWorkouts = new ObservableCollection<LiftWorkoutViewModel>(_workout.LiftWorkouts.Select(lw => new LiftWorkoutViewModel(lw)));
            Notes = _workout.Notes;
        }
       
        [ObservableProperty]
        string name;

        [ObservableProperty]
        string notes;

        [ObservableProperty]
        DateTime dateTime;

        [ObservableProperty]
        ObservableCollection<LiftWorkoutViewModel> liftWorkouts;

        public void Merge(WorkoutViewModel workout)
        {
            foreach (var lw in workout.LiftWorkouts)
            {
                if (LiftWorkouts.Select(mlw => mlw.Lift).Contains(lw.Lift))
                {
                    var common = LiftWorkouts.ToList().Find(_ => _.Lift == lw.Lift);

                    foreach (var set in lw.Sets)
                    {
                        common!.Sets.Add(set);
                    }
                }
                else
                {
                    LiftWorkouts.Add(lw);
                }
            }
        }

        public override WorkoutDTO ToModel()
        {
            _workout.Name = Name;
            _workout.DateTime = DateTime;
            _workout.LiftWorkouts = LiftWorkouts.ToModels().ToList();
            _workout.Notes = Notes;

            return _workout;
        }
    }
}
