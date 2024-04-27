using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGymApp.Models;
using MauiGymApp.Models.DTOs.Measurables;
using MauiGymApp.State;
using MauiGymApp.ViewModels.Common;

namespace MauiGymApp.ViewModels.MeasurableQuantities
{
    public partial class AddMeasurableQuantityViewModel : BaseViewModel
    {
        private readonly IMeasurableStateService _stateService;

        public AddMeasurableQuantityViewModel(IMeasurableStateService stateService)
        {
            _stateService = stateService;
            SelectedQuantityType = QuantityType.Length.ToString();
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNameNotEmpty))]
        string name = "";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(QuantityType))]
        string selectedQuantityType;

        public bool IsNameNotEmpty => !string.IsNullOrEmpty(Name);

        QuantityType QuantityType => Enum.Parse<QuantityType>(SelectedQuantityType); 

        [RelayCommand]
        async Task ConfirmAsync()
        {
            if (!IsNameNotEmpty)
                return;

            MeasurableQuantityDTO quantity = new()
            {
                Name = Name,
                QuantityType = QuantityType,
                DateCreated = DateTime.Now,
                Notes = "",
            };

            await _stateService.AddMeasurableQuantity(new MeasurableQuantityViewModel(quantity));
            await Shell.Current.GoToAsync("..", animate: true);
        }

        [RelayCommand]
        async static Task CancelAsync() => await Shell.Current.GoToAsync("..");
    }
}
