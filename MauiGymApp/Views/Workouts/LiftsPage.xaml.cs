using MauiGymApp.ViewModels.Workouts.Lifts;

namespace MauiGymApp.Views.Workouts;

public partial class LiftsPage : ContentPage
{
	public LiftsPage(LiftsPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}