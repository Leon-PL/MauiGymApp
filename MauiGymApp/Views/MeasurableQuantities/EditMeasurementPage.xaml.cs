using MauiGymApp.ViewModels.MeasurableQuantities;

namespace MauiGymApp.Views.MeasurableQuantities;

public partial class EditMeasurementPage : ContentPage
{
	public EditMeasurementPage(EditMeasurementViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}