using MauiGymApp.ViewModels.Calculator;

namespace MauiGymApp.Views;

public partial class CalculatorPage : ContentPage
{
	public CalculatorPage(CalculatorViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}