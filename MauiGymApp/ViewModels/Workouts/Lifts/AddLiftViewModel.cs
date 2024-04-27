using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGymApp.Models;
using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.Services.WeightLifting;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Utilities;
using System.Collections.ObjectModel;

namespace MauiGymApp.ViewModels.Workouts.Lifts
{
    public partial class AddLiftViewModel : BaseViewModel
    {
        private readonly ILiftService _liftService;

        public AddLiftViewModel(ILiftService liftService)
        {
            _liftService = liftService;

            MovementPatternPickerOptions = new ObservableCollection<string>(Enum.GetValues<MovementPattern>().Select(mp => mp.FormatAsString()));
            SelectedMovementPattern = MovementPatternPickerOptions.First();        
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ConfirmCommand))]
        string name;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(MovementPattern))]
        string selectedMovementPattern;

        [ObservableProperty]
        public ObservableCollection<string> movementPatternPickerOptions;

        public MovementPattern MovementPattern => ViewModelHelper.MovementPatternFromFormattedString(SelectedMovementPattern);

        public bool IsValidLift => !string.IsNullOrWhiteSpace(Name);

        [RelayCommand(CanExecute = nameof(IsValidLift))]
        async Task ConfirmAsync()
        {
            LiftDTO newLift = new()
            {
                DateCreated = DateTime.Now,
                Name = Name,
                MovementPattern = MovementPattern,
                E1RMGoal = null,
            };

            await _liftService.AddAsync(newLift);
            await Shell.Current.GoToAsync("../", animate: true);
        }

        [RelayCommand]
        async static Task CancelAsync() => await Shell.Current.GoToAsync("..", animate: true);
    }
}
