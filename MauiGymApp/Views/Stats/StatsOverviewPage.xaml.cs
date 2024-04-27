using MauiGymApp.ViewModels.Stats;

namespace MauiGymApp.Views.Stats;

public partial class StatsOverviewPage : ContentPage
{
	public StatsOverviewPage(StatsOverviewViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}