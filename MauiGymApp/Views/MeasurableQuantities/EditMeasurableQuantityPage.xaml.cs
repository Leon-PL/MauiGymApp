using MauiGymApp.ViewModels.MeasurableQuantities;

namespace MauiGymApp.Views.MeasurableQuantities;

public partial class EditMeasurableQuantityPage : ContentPage
{
	public EditMeasurableQuantityPage(EditMeasurableQuantityViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm; 
	}
}