using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Maui.Core.Internal;
using MauiGymApp.Nito;
using MauiGymApp.Services.WeightLifting;
using MauiGymApp.ViewModels.Common;
using System.Collections.ObjectModel;

namespace MauiGymApp.ViewModels.Workouts.Lifts
{
    public partial class LiftsPopUpViewModel : BaseViewModel
    {
        private readonly ILiftService _liftService;

        public event Action<IEnumerable<LiftViewModel>>? LiftsSelected;
        public event Action? Closed;

        bool isOpen;

        public LiftsPopUpViewModel(ILiftService liftService)
        {
            _liftService = liftService;

            selectedLifts = [];
            Lifts = NotifyTask.Create(LoadLifts(), []);
            IsOpen = false;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LiftGroups))]
        NotifyTask<ObservableCollection<LiftViewModel>> lifts;

        public async Task<ObservableCollection<LiftViewModel>> LoadLifts()
        {
            var lifts = await _liftService.GetAllAsync();
            return new(lifts.Select(l => new LiftViewModel(l)));
        }

        public IEnumerable<LiftGroupViewModel> LiftGroups => GetGroups();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        IList<object> selectedLifts;

        public bool HasSelected => !SelectedLifts.Any();

        public bool IsOpen
        {
            get => isOpen;
            set 
            {
                isOpen = value;
                OnPropertyChanged();
                if (!value) OnClosed();
                else SelectedLifts = [];
            }
        }

        public IEnumerable<LiftGroupViewModel> GetGroups()
        {
            if (Lifts is null || Lifts.IsNotCompleted) return [];
            return LiftGroupViewModel.CreateLiftGroups(Lifts.Result);
        }

        [RelayCommand]
        void ToggleGroupExpanded(LiftGroupViewModel group)
         {
            group.IsExpanded = !group.IsExpanded;

            if (!group.IsExpanded)
            {
                group.Clear();
            }
            else
            {
                var lifts = Lifts.Result.Where(l => l.MovementPattern == group.MovementPattern);
                group.InsertRange(0, lifts);
            }
        }

        [RelayCommand(CanExecute = nameof(HasSelected))]
        void Confirm()
        {   
            LiftsSelected?.Invoke(SelectedLifts.Cast<LiftViewModel>());
            IsOpen = false;
        }

        [RelayCommand]
        void Cancel() => IsOpen = false;

        public void OnClosed() => Closed?.Invoke();

        public override Task GoToAddLiftAsync()
        {
            IsOpen = false; 
            return base.GoToAddLiftAsync();
        }
    }
}
