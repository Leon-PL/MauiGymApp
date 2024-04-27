using MauiGymApp.ViewModels.MeasurableQuantities;

namespace MauiGymApp.Views.MeasurableQuantities;

public partial class MeasurementsPage : ContentPage
{
    MeasurementsViewModel _vm;

    public MeasurementsPage(MeasurementsViewModel vm)
    {
        InitializeComponent();

        _vm = vm;
        BindingContext = _vm;
    }

    private void Delete_SwipeItem_Invoked(object sender, EventArgs e)
    {
        SwipeItem item = (SwipeItem)sender;
        var t = item.BindingContext;
        var vm = (MeasurementDifferentialViewModel)item.BindingContext;
        _vm.DeleteMeasurementCommand.Execute(vm);
    }
}