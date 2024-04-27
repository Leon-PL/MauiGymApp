using MauiGymApp.ViewModels.Workouts.Lifts;

namespace MauiGymApp.Views.Workouts;

public partial class AddLiftPage : ContentPage
{
	public AddLiftPage(AddLiftViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}