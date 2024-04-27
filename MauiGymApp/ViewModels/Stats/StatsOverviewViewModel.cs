using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGymApp.State;
using MauiGymApp.ViewModels.Common;

namespace MauiGymApp.ViewModels.Stats
{
    public partial class StatsOverviewViewModel : BaseViewModel
    {   
        private readonly IWorkoutStateService _workoutStateService;

        public StatsOverviewViewModel(IWorkoutStateService stateService)
        {
            _workoutStateService = stateService;

            MovementPatternStatsViewModel = new();
            WorkoutStatsViewModel = new(_workoutStateService.Workouts);

            SelectedTab = 1;
        }

        [ObservableProperty]
        WorkoutStatsViewModel workoutStatsViewModel;

        [ObservableProperty]
        MovementPatternStatsViewModel movementPatternStatsViewModel;

        [ObservableProperty]
        int selectedTab;

        [RelayCommand]
        void SelectItem(string param) => SelectedTab = int.Parse(param);
    }
}
