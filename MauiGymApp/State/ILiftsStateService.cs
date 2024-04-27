using MauiGymApp.ViewModels.Workouts.Lifts;

namespace MauiGymApp.State
{
    public interface ILiftsStateService
    {
        public event Action? LiftsChanged;
        public bool Loaded { get; }
        public IEnumerable<LiftViewModel> Lifts { get; }

        public event Action<IEnumerable<LiftViewModel>>? LiftsConfirmedSelected;
        public void SelectLifts(IEnumerable<LiftViewModel> lifts);

        public Task AddLift(LiftViewModel lift);
        public Task AddLifts(IEnumerable<LiftViewModel> lifts);
    }
}
