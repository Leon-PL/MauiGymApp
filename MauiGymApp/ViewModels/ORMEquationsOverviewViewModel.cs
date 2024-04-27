using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using MauiGymApp.Calculations;
using MauiGymApp.Services.Calculator;
using MauiGymApp.Services.Settings;
using MauiGymApp.ViewModels.Common;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiGymApp.ViewModels
{
    public partial class ORMEquationsOverviewViewModel : BaseViewModel
    {   
        private readonly ICalculatorService _calculatorService;
        private readonly ISettingsService _settingsService;

        private static readonly SKColor s_gray = new(195, 195, 195);
        private static readonly SKColor s_gray1 = new(160, 160, 160);
        private static readonly SKColor s_gray2 = new(90, 90, 90);
        private static readonly SKColor s_dark3 = new(60, 60, 60);

        public IEnumerable<int> RepRange = Enumerable.Range(1, 35);

        public ORMEquationsOverviewViewModel(ICalculatorService calculatorService, ISettingsService settingsService)
        {
            _calculatorService = calculatorService;
            _settingsService = settingsService;

            EpleyActive = _settingsService.UseEpley;
            BryzckiActive = _settingsService.UseBryzcki;
            LombardiActive = _settingsService.UseLombardi;
            MayhewActive = _settingsService.UseMayhew;
            OConnorActive  = _settingsService.UseOConnor;
            WathanActive = _settingsService.UseWathan;
        }

        #region Active Booleans
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Series))]
        bool epleyActive;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Series))]
        bool bryzckiActive;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Series))]
        bool lombardiActive;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Series))]
        bool mayhewActive;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Series))]
        bool oConnorActive;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Series))]
        bool wathanActive;

        #endregion

        #region Colours
        [ObservableProperty]
        Color epleyColour = Color.FromRgb(255, 0, 0);

        [ObservableProperty]
        Color bryzckiColour = Color.FromRgb(0, 255, 0);

        [ObservableProperty]
        Color lombardiColour = Color.FromRgb(0, 0, 255);

        [ObservableProperty]
        Color mayhewColour = Color.FromRgb(100, 50, 043);

        [ObservableProperty]
        Color oConnorColour = Color.FromRgb(13, 100, 183);

        [ObservableProperty]
        Color wathanColour = Color.FromRgb(83, 200, 143);

        #endregion

        public ISeries EpleySeries => new LineSeries<ObservablePoint>
        {
            Values = GetPoints([OneRepMaxFunctions.Epley]),
            Stroke = new SolidColorPaint(EpleyColour.ToSKColor(), 4),
            Fill = null,
            GeometrySize = 0,
            Name = "Epley"
        };

       
        public ISeries BryzckiSeries => new LineSeries<ObservablePoint>
        {
            Values = GetPoints([OneRepMaxFunctions.Bryzki]),
            Stroke = new SolidColorPaint(BryzckiColour.ToSKColor(), 4),
            Fill = null,
            GeometrySize = 0,
            Name = "Bryzcki"
        };

        public ISeries LombardiSeries => new LineSeries<ObservablePoint>
        {
            Values = GetPoints([OneRepMaxFunctions.Lombardi]),
            Stroke = new SolidColorPaint(LombardiColour.ToSKColor(), 4),
            Fill = null,
            GeometrySize = 0,
            Name = "Lombardi"
        };

        public ISeries MayhewSeries => new LineSeries<ObservablePoint>
        {
            Values = GetPoints([OneRepMaxFunctions.Mayhew]),
            Stroke = new SolidColorPaint(MayhewColour.ToSKColor(), 4),
            Fill = null,
            GeometrySize = 0,
            Name = "Mayhew"
        };

        public ISeries OConnorSeries => new LineSeries<ObservablePoint>
        {
            Values = GetPoints([OneRepMaxFunctions.OConnor]),
            Stroke = new SolidColorPaint(OConnorColour.ToSKColor(), 4),
            Fill = null,
            GeometrySize = 0,
            Name = "OConnor"
        };

        public ISeries WathanSeries => new LineSeries<ObservablePoint>
        {
            Values = GetPoints([OneRepMaxFunctions.Wathan]),
            Stroke = new SolidColorPaint(WathanColour.ToSKColor(), 4),
            Fill = null,
            GeometrySize = 0,
            Name = "Wathan"
        };


        public List<ISeries> GetActiveSeries()
        {
            List<ISeries> active = [];

            if (EpleyActive) active.Add(EpleySeries);
            if (BryzckiActive) active.Add(BryzckiSeries);
            if (LombardiActive) active.Add(LombardiSeries);
            if (MayhewActive) active.Add(MayhewSeries);
            if (OConnorActive) active.Add(OConnorSeries);
            if (WathanActive) active.Add(WathanSeries);

            return active;
        }

        public List<OneRepMaxFunction> GetActiveFunctions()
        {
            List<OneRepMaxFunction> active = [];

            if (EpleyActive) active.Add(OneRepMaxFunctions.Epley);
            if (BryzckiActive) active.Add(OneRepMaxFunctions.Bryzki);
            if (LombardiActive) active.Add(OneRepMaxFunctions.Lombardi);
            if (MayhewActive) active.Add(OneRepMaxFunctions.Mayhew);
            if (OConnorActive) active.Add(OneRepMaxFunctions.OConnor);
            if (WathanActive) active.Add(OneRepMaxFunctions.Wathan);

            return active;
        }

        public ISeries[] Series
        {
            get
            {
                var active = GetActiveSeries();

                if (active.Count > 1)
                    active.Add(AverageSeries());
                return active.ToArray();
            }
        }

        public IEnumerable<ObservablePoint> GetPoints(IEnumerable<OneRepMaxFunction> functions)
        {
            var weights = RepRange.Select(r => _calculatorService.WeightMax(100, r, functions));
            var percentages = weights.Select(w => 100 * w / weights.Max()).ToList();
            return RepRange.Select(r => new ObservablePoint(r, percentages[r - 1]));
        }
        public ISeries AverageSeries()
        {

            return new LineSeries<ObservablePoint>
            {
                Values = GetPoints(GetActiveFunctions()),
                Stroke = new SolidColorPaint(new SKColor(255, 255, 255), 4),
                Fill = null,
                GeometrySize = 0,
                Name = "Average"
            };
        }

        public Axis[] XAxes { get; set; } =
        {
        new Axis
            {
                Name = "Reps",
                NamePaint = new SolidColorPaint(s_gray1),
                TextSize = 12,
                NameTextSize = 12,
                Padding = new Padding(5, 15, 5, 5),
                LabelsPaint = new SolidColorPaint(s_gray),
                SeparatorsPaint = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1,
                    PathEffect = new DashEffect(new float[] { 3, 3 })
                },
                SubseparatorsPaint = new SolidColorPaint
                {
                    Color = s_gray2,
                    StrokeThickness = 0.5f
                },
                SubseparatorsCount = 9,
                ZeroPaint = new SolidColorPaint
                {
                    Color = s_gray1,
                    StrokeThickness = 2
                },
                TicksPaint = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1.5f
                },
                SubticksPaint = new SolidColorPaint
                {
                    Color = s_gray,
                    StrokeThickness = 1
                }
            }
        };

        public Axis[] YAxes { get; set; } =
        {
        new Axis
        {
            Name = "% One Rep Max",
            NamePaint = new SolidColorPaint(s_gray1),
            TextSize = 12,
            NameTextSize=12,
            MinLimit=0,
            MaxLimit=101,
            Padding = new Padding(0, 0, 10, 0),
            LabelsPaint = new SolidColorPaint(s_gray),
            SeparatorsPaint = new SolidColorPaint
            {
                Color = s_gray,
                StrokeThickness = 1,
                PathEffect = new DashEffect(new float[] { 3, 3 })
            },
            SubseparatorsPaint = new SolidColorPaint
            {
                Color = s_gray2,
                StrokeThickness = 0.5f
            }, 
            SubseparatorsCount = 9,
            ZeroPaint = new SolidColorPaint
            {
                Color = s_gray1,
                StrokeThickness = 2
            },
            TicksPaint = new SolidColorPaint
            {
                Color = s_gray,
                StrokeThickness = 1.5f
            },
            SubticksPaint = new SolidColorPaint
            {
                Color = s_gray,
                StrokeThickness = 1
            }
            
        }
    };

        public DrawMarginFrame Frame { get; set; } =
        new()
        {
            Fill = new SolidColorPaint(s_dark3),
            Stroke = new SolidColorPaint
            {
                Color = s_gray,
                StrokeThickness = 1
            }
        };

        private static List<ObservablePoint> Fetch()
        {
            var list = new List<ObservablePoint>();
            var fx = EasingFunctions.BounceInOut;

            for (var x = 0f; x < 1f; x += 0.001f)
            {
                var y = fx(x);
                list.Add(new ObservablePoint(x - 0.5, y - 0.5));
            }

            return list;
        }

        private static List<ObservablePoint> Fetch2()
        {
            var list = new List<ObservablePoint>();
            var fx = EasingFunctions.BounceInOut;

            for (var x = 0f; x < 1f; x += 0.001f)
            {
                var y = fx(x);
                list.Add(new ObservablePoint(x - 0.25, y - 0.25));
            }

            return list;
        }

        [RelayCommand]
        public async Task GoToORMEquationsWiki()
        {
            try
            {       
                Uri uri = new("https://en.wikipedia.org/wiki/One-repetition_maximum");
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                await DisplayGenericErrorPrompt(ex.Message);
            }
        }

        public List<ObservablePoint> GetPoints(OneRepMaxFunction function)
        {
            throw new NotImplementedException();
        }
    }
}
