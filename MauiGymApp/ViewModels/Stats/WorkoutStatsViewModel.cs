using CommunityToolkit.Mvvm.ComponentModel;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Workouts;

namespace MauiGymApp.ViewModels.Stats
{
    public partial class WorkoutStatsViewModel : BaseViewModel
    {
        public WorkoutStatsViewModel(IEnumerable<WorkoutViewModel> workouts)
        {
            Workouts = workouts;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalSets))]
        IEnumerable<WorkoutViewModel> workouts;

        public int TotalWorkouts => Workouts.Count();
        public int TotalSets => Workouts.SelectMany(w => w.LiftWorkouts.SelectMany(lw => lw.Sets)).Count();
    }
}
