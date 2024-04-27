using MauiGymApp.Views;
using MauiGymApp.Views.Home;
using MauiGymApp.Views.MeasurableQuantities;
using MauiGymApp.Views.Stats;
using MauiGymApp.Views.Workouts;

namespace MauiGymApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(CalendarPage), typeof(CalendarPage));
            Routing.RegisterRoute(nameof(AddWorkoutPage), typeof(AddWorkoutPage));
            Routing.RegisterRoute(nameof(CopyPreviousWorkoutPage), typeof(CopyPreviousWorkoutPage));
            Routing.RegisterRoute(nameof(LiftOverviewPage), typeof(LiftOverviewPage));
            Routing.RegisterRoute(nameof(LiftsPage), typeof(LiftsPage));
            Routing.RegisterRoute(nameof(StatsOverviewPage), typeof(StatsOverviewPage));
            Routing.RegisterRoute(nameof(AddLiftPage), typeof(AddLiftPage));
            Routing.RegisterRoute(nameof(MeasurableQuantitiesPage), typeof(MeasurableQuantitiesPage));
            Routing.RegisterRoute(nameof(AddMeasurableQuantityPage), typeof(AddMeasurableQuantityPage));
            Routing.RegisterRoute(nameof(EditMeasurableQuantityPage), typeof(EditMeasurableQuantityPage));
            Routing.RegisterRoute(nameof(MeasurementsPage), typeof(MeasurementsPage));
            Routing.RegisterRoute(nameof(AddMeasurementPage), typeof(AddMeasurementPage));
            Routing.RegisterRoute(nameof(EditMeasurementPage), typeof(EditMeasurementPage));
            Routing.RegisterRoute(nameof(CalculatorPage), typeof(CalculatorPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(ORMEquationsOverviewPage), typeof(ORMEquationsOverviewPage));
        }
    }
}
