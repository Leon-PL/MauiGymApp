using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGymApp.Models;
using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.Services.Visual;
using MauiGymApp.State;
using MauiGymApp.ViewModels.Common;
using System.Collections.ObjectModel;

namespace MauiGymApp.ViewModels.Workouts.Lifts
{   
    public partial class LiftsPageViewModel : BaseViewModel
    {
        private readonly IApplicationStateService _applicationStateService;
        private readonly ILiftsStateService _stateService;
        private readonly IWorkoutStateService _workoutStateService;

        public event Action<IEnumerable<LiftViewModel>>? LiftsConfirmed;

        public LiftsPageViewModel(IApplicationStateService applicationStateService,ILiftsStateService stateService, IWorkoutStateService workoutStateService)
        {   
            _applicationStateService = applicationStateService;
            _stateService = stateService;
            _workoutStateService = workoutStateService;

            if (_stateService.Loaded) Lifts = _stateService.Lifts.ToList();

            _stateService.LiftsChanged += () => Lifts = _stateService.Lifts.ToList();
        }

        [ObservableProperty]
        List<LiftViewModel> lifts = [];

        [ObservableProperty]
        IEnumerable<object>? selectedLifts;

        [ObservableProperty]
        bool showConfirmButton;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PatternFilteredLifts))]
        [NotifyPropertyChangedFor(nameof(SearchFilteredLifts))]
        string selectedPatternText = "All";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SearchFilteredLifts))]
        string searchText = "";


        public List<LiftViewModel> SearchFilteredLifts 
        { 
            get
            {
                if (string.IsNullOrWhiteSpace(SearchText)) return PatternFilteredLifts;
                return PatternFilteredLifts
                    .Where(l => l.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            } 
        }

        public List<LiftViewModel> PatternFilteredLifts 
        {
            get 
            {   
                if (SelectedPatternText == "All") return Lifts;
                var pattern = (MovementPattern)Enum.Parse(typeof(MovementPattern), SelectedPatternText);
                return Lifts.Where(l => l.MovementPattern == pattern).ToList();
            }
        }

        [RelayCommand]
        async Task OpenLiftActionSheet(LiftViewModel lift)
        {
            var action = await Shell.Current.DisplayActionSheet("Action", "Cancel", null, "View", "Edit", "Delete");

            if (action == "View")
            {
                await GoToLiftAsync(lift);
            }

            if (action == "Delete")
            {
                await SnackBarService.ShowSnackBar($"{lift.Name} removed from workout", SnackBarType.Success);
            }
        }

        [RelayCommand]
        void SelectedLiftsChanged() => ShowConfirmButton = SelectedLifts?.Count() > 0;

        [RelayCommand]
        void Confirm() => LiftsConfirmed?.Invoke((SelectedLifts!.Cast<LiftViewModel>()));

        [ObservableProperty]
        public List<string> pickerOptions = Enum.GetNames(typeof(MovementPattern)).Prepend("All").ToList();

        public override void Dispose()
        {
            base.Dispose();
            _stateService.LiftsChanged -= () => Lifts = _stateService.Lifts.ToList();

        }
    }
}
