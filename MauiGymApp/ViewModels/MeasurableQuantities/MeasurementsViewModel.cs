using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using MauiGymApp.Calculations;
using MauiGymApp.Models;
using MauiGymApp.Models.DTOs.Goals;
using MauiGymApp.Models.DTOs.Measurables;
using MauiGymApp.Services.Measurables;
using MauiGymApp.Services.Settings;
using MauiGymApp.State;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Utilities;
using MauiGymApp.Views.MeasurableQuantities;
using System.Collections.Specialized;
using System.ComponentModel;
using UnitsNet;
using UnitsNet.Units;

namespace MauiGymApp.ViewModels.MeasurableQuantities
{
    public partial class MeasurementsViewModel : BaseViewModel
    {
        private readonly IMeasurableStateService _stateService;
        private readonly ISettingsService _settingsService;

        public ISeries[] Series => [ new LineSeries<DateTimePoint>
        {
            Values = MeasurableQuantity.Measurements.ToDateTimePoints(m => m.DateTime, m => _settingsService.IQuantityAsPreferredUnit(m.Value)),
            GeometrySize = 1,
            DataPadding = new LvcPoint(0, 0),

        }];

        [ObservableProperty]
        public Axis[] xAxes = [ new Axis
        {
            Labeler = value => new DateTime((long)value).ToString("MMMM dd"),
            LabelsRotation = 15,
            TextSize = 10,
            UnitWidth = TimeSpan.FromDays(1).Ticks,
            MinStep = TimeSpan.FromDays(1).Ticks
        }];


        [ObservableProperty]
        public Axis[] yAxes = [new Axis { TextSize = 10, MinStep = 1 }];

        public MeasurementsViewModel(IMeasurableStateService stateService, ISettingsService settingsService)
        {
            _stateService = stateService;
            _settingsService = settingsService;

            MeasurableQuantity = _stateService.MeasurableQuantityQuery!;

            MeasurableQuantity.PropertyChanged += OnMeasurableQuantityChanged;
            MeasurableQuantity.Measurements.CollectionChanged += OnMeasurementsChanged;
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MeasurementDifferentials))]
        MeasurableQuantityViewModel measurableQuantity;

        public IEnumerable<MeasurementDifferentialViewModel> MeasurementDifferentials 
            => MeasurableQuantity.GetDifferentials(_settingsService.GetUnitPreference(MeasurableQuantity.QuantityType));

        [ObservableProperty]
        QuantityType quantityType;

        [ObservableProperty]
        bool isGoalPromptOpen;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(GoalString))]
        [NotifyPropertyChangedFor(nameof(RemainingString))]
        GoalProgressViewModel? goalProgress;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(GoalEditorQuantity))]
        string? goalEditorText;

        public bool CanOpenAddGoalPrompt => RecentMeasurement is not null;

        public Enum MeasurementUnit =>  _settingsService.GetUnitPreference(MeasurableQuantity.QuantityType);

        public string MeasurementUnitString
        {
            get
            {
                if (MeasurableQuantity.QuantityType == QuantityType.Percentage) return "%";
                return  UnitsNetHelpers.GetUnitAbreviation(MeasurementUnit);
            }
        }

        public IQuantity? GoalEditorQuantity
        {
            get
            {
                if (GoalEditorText is null) return null;
                return Quantity.From(double.Parse(GoalEditorText), MeasurementUnit);
            }
        }

        public bool HasGoal => MeasurableQuantity.Goal is not null;
        
        public string? GoalString => MeasurementUnit is not null ?
            MeasurableQuantity?.Goal?.TargetValue.ToUnit(MeasurementUnit).ToString() : null;

        public string? RemainingString => MeasurementUnit is not null ?
            GoalProgress?.Remaining.ToUnit(MeasurementUnit).ToString() : null;

        public IQuantity? GoalMeasurement => MeasurableQuantity?.Goal?.TargetValue;

        public IQuantity? RecentMeasurement => MeasurableQuantity?.Measurements?.Any() is true ? 
            MeasurableQuantity?.Measurements?.OrderBy(m => m.DateTime).Last().Value : null;

        void UpdateGoalProgress()
        {
            if (GoalMeasurement is null || RecentMeasurement is null)
            {
                GoalProgress = null;
                return;
            }
            var goalProgress = new GoalProgress(GoalMeasurement, RecentMeasurement);
            GoalProgress?.UpdateProgress(goalProgress);
        }

        [RelayCommand]
        async Task OpenActionSheetAsync()
        {
            string action = await Shell.Current.DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");
            if (action == "Delete")
            {   
                await DeleteMeasurableQuantityAsync(MeasurableQuantity);
                await Shell.Current.GoToAsync("../");
            }

            if (action == "Edit") await GoToEditMeasurableQuantityAsync();
        }

        [RelayCommand]
        async Task GoToEditMeasurableQuantityAsync() => await _stateService.GoToEditMeasurableQuantityAsync(MeasurableQuantity);

        [RelayCommand]
        async Task DeleteMeasurableQuantityAsync(MeasurableQuantityViewModel quantity)
        {
            if (quantity is null)
                await DisplayGenericErrorPrompt();

            else
            {
                bool confirm = await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete", "Confirm", "Cancel");
                if (confirm)
                {
                    await _stateService.DeleteMeasurableQuantity(quantity);
                    await Shell.Current.GoToAsync("..", animate: true);
                }
            }
        }

        [RelayCommand]
        void OpenAddGoal() => IsGoalPromptOpen = true;

        [RelayCommand]
        async Task AddGoalAsync()
        {  
            if (GoalEditorQuantity is null) return;

            var dto = new GoalDTO()
            {   
                TargetValueSI = GoalEditorQuantity.AsBaseUnit(),
                QuantityType = MeasurableQuantity.QuantityType,
            };

            MeasurableQuantity.Goal = new GoalViewModel(dto);
            await _stateService.UpdateMeasurableQuantity(MeasurableQuantity);
            UpdateGoalProgress();
            IsGoalPromptOpen = false;
        }

        [RelayCommand]
        async Task GoToAddMeasurementAsync() => await _stateService.GoToAddMeasurementAsync(MeasurableQuantity);

        [RelayCommand]
        async Task GoToEditMeasurementAsync(MeasurementDifferentialViewModel measurement)
            => await _stateService.GoToEditMeasurementAsync(MeasurableQuantity, measurement);

        [RelayCommand]
        async Task DeleteMeasurement(MeasurementDifferentialViewModel measurement)
        {
            bool confirm = await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete", "Confirm", "Cancel");
            if (confirm)
            {
                await _stateService.DeleteMeasurementAsync(measurement);
            }
        }

        private void OnMeasurableQuantityChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MeasurableQuantityViewModel.Measurements) || e.PropertyName == nameof(MeasurableQuantity.Goal))
            {
                OnPropertyChanged(nameof(MeasurableQuantity));
                OnPropertyChanged(nameof(Series));
                UpdateGoalProgress();
            }
        }

        private void OnMeasurementsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(MeasurableQuantity));
            OnPropertyChanged(nameof(Series));
            UpdateGoalProgress();
        }

        public override void Dispose()
        {
            MeasurableQuantity.PropertyChanged -= OnMeasurableQuantityChanged;
            MeasurableQuantity.Measurements.CollectionChanged -= OnMeasurementsChanged;
            base.Dispose();
        }
    }
}
