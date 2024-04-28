using DevExpress.Maui;
using CommunityToolkit.Maui;
using MauiGymApp.Contexts;
using MauiGymApp.Services.Calculator;
using MauiGymApp.Services.Measurables;
using MauiGymApp.Services.Settings;
using MauiGymApp.ViewModels;
using MauiGymApp.ViewModels.MeasurableQuantities;
using MauiGymApp.Views;
using MauiGymApp.Views.Home;
using MauiGymApp.Views.MeasurableQuantities;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using MauiGymApp.Services.WeightLifting;
using MauiGymApp.ViewModels.Workouts.Lifts;
using MauiGymApp.ViewModels.Workouts;
using MauiGymApp.Views.Workouts;
using MauiGymApp.Services.ImportData;
using MauiGymApp.ViewModels.Home;
using MauiGymApp.State;
using MauiGymApp.ViewModels.Calculator;
using MauiGymApp.Views.Stats;
using MauiGymApp.ViewModels.Stats;
using MemoryToolkit.Maui;

namespace MauiGymApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp(true)
                .UseDevExpress()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

           
            DevExpress.Maui.CollectionView.Initializer.Init();
            DevExpress.Maui.Controls.Initializer.Init();
            DevExpress.Maui.Editors.Initializer.Init();
            DevExpress.Maui.DataGrid.Initializer.Init();
          
            builder.Services.AddDbContext<MainContext>();

            builder.Services.AddTransient(typeof(ICalculatorService), typeof(CalculatorService))
                            .AddSingleton(typeof(ISettingsService), typeof(SettingsService))
                            .AddTransient(typeof(IMeasurablesService), typeof(MeasurablesService))
                            .AddTransient(typeof(ILiftService), typeof(LiftService))
                            .AddTransient(typeof(ILiftWorkoutService), typeof(LiftWorkoutService))
                            .AddTransient(typeof(IRoutineService), typeof(RoutineService))
                            .AddTransient(typeof(IWorkoutService), typeof(WorkoutService))
                            .AddTransient(typeof(IImportDataService), typeof(FitNotesService))
                            .AddSingleton(typeof(IMeasurableStateService), typeof(MeasurableStateService))
                            .AddSingleton(typeof(IWorkoutStateService), typeof(WorkoutStateService))
                            .AddSingleton(typeof(IRoutinesStateService), typeof(RoutineStateService))
                            .AddSingleton(typeof(ILiftsStateService), typeof(LiftsStateService))
                            .AddSingleton(typeof(IApplicationStateService), typeof(ApplicationStateService));

            builder.Services.AddTransient<HomePage>()
                            .AddTransient<HomeViewModel>()

                            .AddTransient<CalendarPage>()
                            .AddTransient<CalendarViewModel>()

                            .AddTransient<LiftsPage>()
                            .AddTransient<LiftsPageViewModel>()

                            .AddTransient<StatsOverviewPage>()
                            .AddTransient<StatsOverviewViewModel>()

                            .AddTransient<AddWorkoutPage>()
                            .AddTransient<AddWorkoutViewModel>()

                            .AddTransient<CopyPreviousWorkoutPage>()
                            .AddTransient<CopyPreviousWorkoutViewModel>()

                            .AddTransient<LiftOverviewPage>()
                            .AddTransient<LiftOverviewViewModel>()
                            .AddTransient<LiftHistoryViewModel>()
                            .AddTransient<LiftGraphsViewModel>()
                            .AddTransient<LiftsPopUpViewModel>()

                            .AddTransient<AddLiftPage>()
                            .AddTransient<AddLiftViewModel>()

                            .AddTransient<CalculatorPage>()
                            .AddTransient<CalculatorViewModel>()

                            .AddTransient<MeasurableQuantitiesPage>()
                            .AddTransient<MeasurableQuantitiesViewModel>()

                            .AddTransient<AddMeasurableQuantityPage>()
                            .AddTransient<AddMeasurableQuantityViewModel>()

                            .AddTransient<EditMeasurableQuantityPage>()
                            .AddTransient<EditMeasurableQuantityViewModel>()

                            .AddTransient<MeasurementsPage>()
                            .AddTransient<MeasurementsViewModel>()

                            .AddTransient<AddMeasurementPage>()
                            .AddTransient<AddMeasurementViewModel>()

                            .AddTransient<EditMeasurementPage>()
                            .AddTransient<EditMeasurementViewModel>()

                            .AddTransient<SettingsPage>()
                            .AddTransient<SettingsViewModel>()

                            .AddTransient<ORMEquationsOverviewPage>()
                            .AddTransient<ORMEquationsOverviewViewModel>();



#if DEBUG
            builder.Logging.AddDebug();
            builder.UseLeakDetection(collectionTarget =>
            {
                // This callback will run any time a leak is detected.
                Application.Current?.MainPage?.DisplayAlert("💦Leak Detected💦",
                    $"❗🧟❗{collectionTarget.Name} is a zombie!", "OK");
            });
#endif

            return builder.Build();
        }
    }
}
