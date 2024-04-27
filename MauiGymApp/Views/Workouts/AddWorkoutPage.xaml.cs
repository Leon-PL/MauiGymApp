using MauiGymApp.ViewModels.Workouts;

namespace MauiGymApp.Views.Workouts;

public partial class AddWorkoutPage : ContentPage
{
	public AddWorkoutPage(AddWorkoutViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}