using MauiGymApp.ViewModels.Workouts;

namespace MauiGymApp.Views.Workouts;

public partial class CopyPreviousWorkoutPage : ContentPage
{
	public CopyPreviousWorkoutPage(CopyPreviousWorkoutViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;	
	}
}