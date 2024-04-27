using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGymApp.Services.WeightLifting;
using MauiGymApp.State;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.Views.Home;
using MauiGymApp.Views.Stats;
using MauiGymApp.Views.Workouts;

namespace MauiGymApp.ViewModels.Home
{
    public partial class HomeViewModel : BaseViewModel
    {
        private readonly IWorkoutStateService _workoutStateService;
        private readonly IApplicationStateService _applicationStateService;
        private readonly IWorkoutService _workoutService;

        public HomeViewModel(IApplicationStateService applicationStateService, IWorkoutService workoutService, IWorkoutStateService workoutStateService)
        {
            _applicationStateService = applicationStateService;
            _workoutStateService = workoutStateService;
            _workoutService = workoutService;

            HomePageWorkoutViewModel = new HomePageWorkoutViewModel(_workoutService, _workoutStateService);
            HomePageEmptyViewModel = new HomePageEmptyViewModel();

            SelectedDate = _applicationStateService.SelectedDate;
        }

        public HomePageWorkoutViewModel HomePageWorkoutViewModel { get; }
        public HomePageEmptyViewModel HomePageEmptyViewModel { get; }

        [ObservableProperty]
        DateTime selectedDate;

        partial void OnSelectedDateChanged(DateTime value)
        {
            HomePageWorkoutViewModel.SelectedDate = SelectedDate;
            _applicationStateService.SelectedDate = SelectedDate;
        }

        [RelayCommand]
        void IncrementDate() => SelectedDate = SelectedDate.AddDays(1);

        [RelayCommand]
        void DecrementDate() => SelectedDate = SelectedDate.AddDays(-1);

        [RelayCommand]
        async Task GoToLiftsPage() => await Shell.Current.GoToAsync($"{nameof(LiftsPage)}", animate: true);

        [RelayCommand]
        async Task GoToStatsPage() => await Shell.Current.GoToAsync($"{nameof(StatsOverviewPage)}", animate: true);

        [RelayCommand]
        async Task GoToCalendarPage() => await Shell.Current.GoToAsync($"{nameof(CalendarPage)}", animate: true);
    }
}
