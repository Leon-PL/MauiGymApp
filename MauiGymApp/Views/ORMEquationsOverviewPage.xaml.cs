using MauiGymApp.ViewModels;

namespace MauiGymApp.Views;

public partial class ORMEquationsOverviewPage : ContentPage
{
	public ORMEquationsOverviewPage(ORMEquationsOverviewViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}