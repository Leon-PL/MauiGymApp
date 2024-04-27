using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGymApp.ViewModels.Workouts.Lifts;
using MauiGymApp.Views.Home;
using MauiGymApp.Views.MeasurableQuantities;
using MauiGymApp.Views.Workouts;

namespace MauiGymApp.ViewModels.Common
{
    public abstract partial class BaseViewModel : ObservableObject, IDisposable
    {
        public virtual async Task DisplayGenericErrorPrompt(string message = "Something went wrong ...")
        {
            await Shell.Current.DisplayAlert("Error", message, "Ok");
            
        }

        #region Navigation

        [RelayCommand]
        public virtual async Task GoHomeAsync()
            => await Shell.Current.GoToAsync(nameof(HomePage));

        [RelayCommand]
        public virtual async Task GoToMeasurableQuanitiesAsync()
            => await Shell.Current.GoToAsync(nameof(MeasurableQuantitiesPage));

        [RelayCommand]
        public virtual async Task GoToAddWorkoutAsync()
            => await Shell.Current.GoToAsync(nameof(AddWorkoutPage), animate: true);


        [RelayCommand]
        public virtual async Task GoToCopyPreviousWorkoutAsync()
            => await Shell.Current.GoToAsync(nameof(CopyPreviousWorkoutPage), animate: true);

        [RelayCommand]
        public virtual async Task GoToLiftAsync(LiftViewModel lift)
            => await Shell.Current.GoToAsync(nameof(LiftOverviewPage), animate: true,
            new Dictionary<string, object>
            {
                { "Lift", lift },
            });


        [RelayCommand]
        public virtual async Task GoToAddLiftAsync()
            => await Shell.Current.GoToAsync(nameof(AddLiftPage), animate: true);
        #endregion

        public virtual void Dispose() 
        {
            GC.SuppressFinalize(this);
        }
    }
}
