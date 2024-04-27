using MauiGymApp.Models;
using MauiGymApp.ViewModels.Common;
using UnitsNet;

namespace MauiGymApp.ViewModels
{
    public class GoalProgressViewModel : BaseViewModel
    {
        private GoalProgress _goalProgress { get; set; }

        public GoalProgressViewModel(GoalProgress goalProgress)
        {
            _goalProgress = goalProgress;
        }

        public void UpdateProgress(GoalProgress goalProgress)
        {
            _goalProgress = goalProgress;
            OnPropertyChanged(nameof(Progress));
            OnPropertyChanged(nameof(Remaining));
        }

        public double Progress => _goalProgress.Progress;
        public IQuantity Remaining => _goalProgress.Remaining;


    }
}
