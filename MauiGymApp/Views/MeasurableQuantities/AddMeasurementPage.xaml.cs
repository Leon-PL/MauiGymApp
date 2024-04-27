using MauiGymApp.ViewModels.MeasurableQuantities;

namespace MauiGymApp.Views.MeasurableQuantities;

public partial class AddMeasurementPage : ContentPage
{
	public AddMeasurementPage(AddMeasurementViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}