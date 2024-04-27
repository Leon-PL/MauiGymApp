using MauiGymApp.ViewModels.MeasurableQuantities;

namespace MauiGymApp.Views.MeasurableQuantities;

public partial class MeasurableQuantitiesPage : ContentPage
{   
    public MeasurableQuantitiesPage(MeasurableQuantitiesViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }


    private void Delete_SwipeItem_Invoked(object sender, EventArgs e)
    {
        SwipeItem item = (SwipeItem)sender;
        MeasurableQuantityViewModel vm = (MeasurableQuantityViewModel)item.BindingContext;
        var _vm = (MeasurableQuantitiesViewModel)BindingContext;
        _vm.DeleteMeasurableQuantityCommand.Execute(vm);
    }
}