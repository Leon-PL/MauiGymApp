using MauiGymApp.Nito;
using MauiGymApp.Services.WeightLifting;
using MauiGymApp.ViewModels.Workouts.Lifts;

namespace MauiGymApp.State
{
    public class LiftsStateService : ILiftsStateService
    {
        private readonly ILiftService _liftService;

        private readonly NotifyTask<IEnumerable<LiftViewModel>> LoadLiftsTask;

        public event Action? LiftsChanged;
        public event Action<IEnumerable<LiftViewModel>>? LiftsConfirmedSelected;

        public bool Loaded => LoadLiftsTask.IsSuccessfullyCompleted;

        public LiftsStateService(ILiftService liftService)
        {
            _liftService = liftService;
            LoadLiftsTask = NotifyTask.Create(Load, []);
        }


        public IEnumerable<LiftViewModel> Lifts { get; private set; } = [];

        public async Task<IEnumerable<LiftViewModel>> Load()
        {
            var lifts = await _liftService.GetAllAsync();
            Lifts = lifts.Select(l => new LiftViewModel(l)).ToList();
            
            LiftsChanged?.Invoke();
            return Lifts;
        }

        public void SelectLifts(IEnumerable<LiftViewModel> lifts) => LiftsConfirmedSelected?.Invoke(lifts);
    }
}
