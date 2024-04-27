using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using MauiGymApp.Calculations;
using MauiGymApp.Models;
using MauiGymApp.Models.DTOs;
using MauiGymApp.Models.DTOs.Goals;
using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.Nito;
using MauiGymApp.Services.Calculator;
using MauiGymApp.Services.Settings;
using MauiGymApp.Services.WeightLifting;
using MauiGymApp.ViewModels.Common;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UnitsNet;
using UnitsNet.Units;

namespace MauiGymApp.ViewModels.Workouts.Lifts
{
    [QueryProperty((nameof(Lift)), (nameof(Lift)))]
    public partial class LiftOverviewViewModel : BaseViewModel
    {
        private readonly ILiftWorkoutService _liftWorkoutService;
        private readonly ILiftService _liftService;
        private readonly ICalculatorService _calculatorService;
        private readonly ISettingsService _settingsService;

        private bool _liftLoaded;

        public LiftOverviewViewModel(ILiftWorkoutService liftWorkoutService, ILiftService liftService, ICalculatorService calculatorService, ISettingsService settingsService)
        {
            _liftWorkoutService = liftWorkoutService;
            _liftService = liftService;
            _calculatorService = calculatorService;
            _settingsService = settingsService;

            SelectedTab = 1;  
        }

        [ObservableProperty]
        LiftHistoryViewModel liftHistoryViewModel;

        [ObservableProperty]
        LiftGraphsViewModel liftGraphsViewModel;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasGoal))]
        [NotifyPropertyChangedFor(nameof(GoalString))]
        [NotifyPropertyChangedFor(nameof(RemainingString))]
        [NotifyPropertyChangedFor(nameof(GoalE1RM))]
        [NotifyPropertyChangedFor(nameof(RecentE1RM))]
        [NotifyPropertyChangedFor(nameof(CanOpenAddGoalPrompt))]
        [NotifyPropertyChangedFor(nameof(WeightUnit))]
        [NotifyPropertyChangedFor(nameof(WeightUnitString))]
        LiftViewModel? lift;

        [ObservableProperty]
        int selectedTab;

        [RelayCommand]
        void SelectItem(string param) => SelectedTab = int.Parse(param);

        [ObservableProperty]
        NotifyTask<ObservableCollection<LiftWorkoutViewModel>?> ? liftWorkouts;

        partial void OnLiftChanged(LiftViewModel? value)
        {
            if (!_liftLoaded)
            {
                _liftLoaded = true;
                LiftWorkouts = NotifyTask.Create(LoadLiftWorkoutsAsync(), null);
            }

            _liftService.LiftEdited += OnLiftEdited; 
        }

        [ObservableProperty]
        bool isGoalPromptOpen;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(GoalString))]
        [NotifyPropertyChangedFor(nameof(RemainingString))]
        GoalProgressViewModel? goalProgress;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(GoalEditorMass))]
        string? goalEditorText;

        public bool CanOpenAddGoalPrompt => RecentE1RM is not null;

        public MassUnit WeightUnit => _settingsService.MassUnit;

        public string WeightUnitString => UnitsNetHelpers.GetUnitAbreviation(WeightUnit);

        public Mass? GoalEditorMass
        {
            get
            {
                if (GoalEditorText is null) return null;
                return Mass.From(double.Parse(GoalEditorText), WeightUnit);
            }
        }

        public bool HasGoal => Lift?.E1RMGoal is not null;

        public string? GoalString => Lift?.E1RMGoal?.TargetValue.ToUnit(WeightUnit).ToString();

        public string? RemainingString => GoalProgress?.Remaining.ToUnit(WeightUnit).ToString();

        public Mass? GoalE1RM => Lift?.E1RMGoal is not null ?
            Mass.FromKilograms(Lift.E1RMGoal.TargetValue.As(MassUnit.Kilogram)) : null;

        public Mass? RecentE1RM => (LiftWorkouts?.Result?.Any() is true ?
            LiftGraphsHelper.GetBestE1RM(LiftWorkouts?.Result?.OrderBy(lw => lw?.DateTime)?.Last(), _calculatorService) : null);

        async Task<ObservableCollection<LiftWorkoutViewModel>?> LoadLiftWorkoutsAsync()
        {
            if (Lift is null) return null;
            var liftWorkouts = await _liftWorkoutService.GetAllAsync();
            var viewModels = liftWorkouts.Where(lw => lw.Lift.Id == Lift.ToModel().Id).Select(lw => new LiftWorkoutViewModel(lw)).OrderBy(lw => lw.DateTime).Reverse();
            var collection = new ObservableCollection<LiftWorkoutViewModel>(viewModels);

            LiftHistoryViewModel = new LiftHistoryViewModel(collection);
            LiftGraphsViewModel = new LiftGraphsViewModel(collection, _calculatorService, _settingsService);
            return collection;
        }

        [RelayCommand]
        void UpdateGoalProgress()
        {
            if (GoalE1RM is null || RecentE1RM is null)
            {
                GoalProgress = null;
                return;
            }
            var goalProgress = new GoalProgress(GoalE1RM, RecentE1RM);
            GoalProgress = new GoalProgressViewModel(goalProgress);
        }

        [RelayCommand]
        public void OpenAddGoal()
        {
            IsGoalPromptOpen = true;
        }

        [RelayCommand]
        async Task AddGoalAsync()
        {
            if (Lift is null || GoalEditorMass is null) return;

            var dto = new GoalDTO()
            {
                TargetValueSI = GoalEditorMass.AsBaseUnit(),
                QuantityType = QuantityType.Mass,
            };

            Lift.E1RMGoal = new GoalViewModel(dto);
            await _liftService.UpdateAsync(Lift.ToModel());
            UpdateGoalProgress();
            IsGoalPromptOpen = false;
        }

        public void OnLiftEdited(LiftDTO lift)
        {
            if (Lift?.ToModel().Id == lift.Id) Lift = new LiftViewModel(lift);
        }

        public override void Dispose()
        {
            _liftService.LiftEdited += OnLiftEdited;

            base.Dispose();
        }
    }
}
