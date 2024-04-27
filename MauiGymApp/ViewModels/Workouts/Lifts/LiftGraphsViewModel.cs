using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using MauiGymApp.Models;
using MauiGymApp.Services.Calculator;
using MauiGymApp.Services.Settings;
using MauiGymApp.ViewModels.Common;
using System.Collections.ObjectModel;

namespace MauiGymApp.ViewModels.Workouts.Lifts
{
    public partial class LiftGraphsViewModel : BaseViewModel
    {
        private readonly ICalculatorService _calculatorService;
        private readonly ISettingsService _settingsService;

        public LiftGraphsViewModel(IEnumerable<LiftWorkoutViewModel> liftWorkouts, ICalculatorService calculatorService, ISettingsService settingsService)
        {
            LiftWorkouts = new ObservableCollection<LiftWorkoutViewModel>(liftWorkouts);

            _calculatorService = calculatorService;
            _settingsService = settingsService;

            Lift = liftWorkouts.First().Lift;
            SelectedGraphTypeString = LiftGraphsHelper.GraphTypePickerOptions.First().Key;

            var a = LiftGraphsHelper.ValuesFromGraphType(LiftWorkouts, LiftGraphsHelper.GraphTypePickerOptions[SelectedGraphTypeString], _calculatorService, _settingsService);
        }

        [ObservableProperty]
        LiftViewModel lift;

        [ObservableProperty]
        ObservableCollection<LiftWorkoutViewModel> liftWorkouts;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Series))]
        GraphType selectedGraphType;

        [ObservableProperty]
        string selectedGraphTypeString;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Series))]
        DatePeriod selectedPeriod;

        public ISeries[] Series => [ new LineSeries<DateTimePoint>
        {
            Values =  LiftGraphsHelper.ValuesFromGraphType(LiftWorkouts, LiftGraphsHelper.GraphTypePickerOptions[SelectedGraphTypeString], _calculatorService, _settingsService),
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

    }
}
