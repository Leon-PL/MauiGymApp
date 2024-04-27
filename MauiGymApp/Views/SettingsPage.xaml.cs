using MauiGymApp.ViewModels;

namespace MauiGymApp.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}