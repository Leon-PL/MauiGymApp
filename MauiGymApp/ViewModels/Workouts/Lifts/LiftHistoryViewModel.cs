using CommunityToolkit.Mvvm.ComponentModel;
using MauiGymApp.ViewModels.Common;
using System.Collections.ObjectModel;

namespace MauiGymApp.ViewModels.Workouts.Lifts
{
    public partial class LiftHistoryViewModel : BaseViewModel
    {
        public LiftHistoryViewModel(IEnumerable<LiftWorkoutViewModel> liftWorkouts)
        {
            LiftWorkouts = new ObservableCollection<LiftWorkoutViewModel>(liftWorkouts);
        }

        [ObservableProperty]
        ObservableCollection<LiftWorkoutViewModel> liftWorkouts;
    }
}
