using MauiGymApp.ViewModels.Home;

namespace MauiGymApp.Views.Home;

public partial class CalendarPage : ContentPage
{
	public CalendarPage(CalendarViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;	
	}
}