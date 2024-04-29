using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGymApp.Models;
using MauiGymApp.Services.Measurables;
using MauiGymApp.State;
using MauiGymApp.ViewModels.Common;

namespace MauiGymApp.ViewModels.MeasurableQuantities
{
    public partial class EditMeasurableQuantityViewModel : BaseViewModel
    {
        private readonly IMeasurableStateService _stateService;

        public EditMeasurableQuantityViewModel(IMeasurableStateService stateService)
        {
            _stateService = stateService;

            MeasurableQuantity = (MeasurableQuantityViewModel)_stateService.GetQueryModel<EditMeasurableQuantityViewModel>();
            Name = MeasurableQuantity.Name;
            SelectedQuantityType = MeasurableQuantity.QuantityType.ToString();
            Notes = MeasurableQuantity.Notes;
        }

        [ObservableProperty]
        MeasurableQuantityViewModel measurableQuantity;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNameNotEmpty))]
        string name = "";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(QuantityType))]
        string selectedQuantityType = "Dimensionless";

        [ObservableProperty]
        string notes = "";

        QuantityType QuantityType => Enum.Parse<QuantityType>(SelectedQuantityType);

        public bool IsNameNotEmpty => !string.IsNullOrEmpty(Name);

        [RelayCommand]
        async Task OpenQuantityTypePrompt()
        {
            string type = await Shell.Current.DisplayActionSheet(title: "Type", cancel: null, destruction: null,
                buttons: Enum.GetNames(typeof(QuantityType)));
            if (type is not null)
                SelectedQuantityType = type;
        }

        [RelayCommand]
        async Task DeleteMeasurableQuantityAsync()
        {
            bool confirm = await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete", "Confirm", "Cancel");
            if (confirm)
            {
                await _stateService.DeleteMeasurableQuantity(MeasurableQuantity);
                await Shell.Current.GoToAsync("..", animate: true);
            }
        }

        [RelayCommand]
        async Task ConfirmAsync()
        {
            MeasurableQuantity.Name = Name;
            MeasurableQuantity.QuantityType = QuantityType;
            MeasurableQuantity.Notes = Notes;

            await _stateService.UpdateMeasurableQuantity(MeasurableQuantity);
            await Shell.Current.GoToAsync("..", animate: true);
        }

        [RelayCommand]
        async static Task CancelAsync() => await Shell.Current.GoToAsync("..");
    }
}
