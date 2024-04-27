using MauiGymApp.ViewModels.MeasurableQuantities;

namespace MauiGymApp.Views.MeasurableQuantities;

public partial class AddMeasurableQuantityPage : ContentPage
{
	public AddMeasurableQuantityPage(AddMeasurableQuantityViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}