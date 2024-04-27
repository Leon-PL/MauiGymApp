using MauiGymApp.ViewModels.Workouts.Lifts;

namespace MauiGymApp;

public partial class LiftOverviewPage : ContentPage
{
	public LiftOverviewPage(LiftOverviewViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm; 
	}
}