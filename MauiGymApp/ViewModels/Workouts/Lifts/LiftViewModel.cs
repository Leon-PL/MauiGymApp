using CommunityToolkit.Mvvm.ComponentModel;
using MauiGymApp.Models;
using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.ViewModels.Common;

namespace MauiGymApp.ViewModels.Workouts.Lifts
{
    public partial class LiftViewModel : DTOViewModel<LiftDTO>
    {
        private readonly LiftDTO _lift;

        public LiftViewModel(LiftDTO lift)
        {
            _lift = lift;

            Name = _lift.Name;
            MovementPattern = _lift.MovementPattern;
            E1RMGoal = _lift.E1RMGoal is not null ? new GoalViewModel(_lift.E1RMGoal) : null;
        }

        [ObservableProperty]
        string name;

        [ObservableProperty]
        MovementPattern movementPattern;

        [ObservableProperty]
        GoalViewModel? e1RMGoal;

        public bool HasGoal => E1RMGoal != null;

        public override LiftDTO ToModel()
        {
            _lift.Name = Name;
            _lift.MovementPattern = MovementPattern;
            _lift.E1RMGoal = E1RMGoal?.ToModel();

            return _lift;
        }
    }
}
