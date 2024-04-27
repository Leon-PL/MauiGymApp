using MauiGymApp.Services;
using MauiGymApp.ViewModels.Home;

namespace MauiGymApp.Views.Home;

public partial class HomePage : ContentPage
{
	public HomePage(HomeViewModel vm)
	{	
		InitializeComponent();
		BindingContext = vm;
	}
}

 