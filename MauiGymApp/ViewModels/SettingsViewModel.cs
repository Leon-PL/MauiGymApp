using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGymApp.Calculations;
using MauiGymApp.Contexts;
using MauiGymApp.Services.ImportData;
using MauiGymApp.Services.Measurables;
using MauiGymApp.Services.Settings;
using MauiGymApp.Services.Visual;
using MauiGymApp.Services.WeightLifting;
using MauiGymApp.State;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.MeasurableQuantities;
using MauiGymApp.ViewModels.Workouts;
using MauiGymApp.Views;

namespace MauiGymApp.ViewModels
{
    public partial class SettingsViewModel : BaseViewModel
    {
        private readonly MainContext _context;
        private readonly ISettingsService _settingsService;
        private readonly IMeasurableStateService _measurableStateService;
        private readonly IWorkoutStateService _workoutStateService;
        private readonly IImportDataService _importService;

        private readonly IWorkoutService _workoutService;

        public SettingsViewModel(MainContext context, ISettingsService settingsService, IMeasurableStateService measurableStateService, IWorkoutStateService workoutStateService, IImportDataService importService, IWorkoutService workoutService)
        {
            _context = context;
            _settingsService = settingsService;
            _measurableStateService = measurableStateService;
            _workoutStateService = workoutStateService;
            _importService = importService;

            _workoutService = workoutService;

            PreferredMassUnit = UnitsNetHelpers.MassUnitPickerOptions.Reverse()[_settingsService.MassUnit];
            PreferredLengthUnit = UnitsNetHelpers.LengthUnitPickerOptions.Reverse()[_settingsService.LengthUnit];

            UseEpley = _settingsService.UseEpley;
            UseBryzcki = _settingsService.UseBryzcki;
            UseLombardi = _settingsService.UseLombardi;
            UseMayhew = _settingsService.UseMayhew;
            UseOConnor = _settingsService.UseOConnor;
            UseWathan = _settingsService.UseWathan;
        }

        #region Units
        [ObservableProperty]
        string preferredMassUnit;

        [ObservableProperty]
        string preferredLengthUnit;

        [ObservableProperty]
        bool loading;

        #endregion

        #region One rep max functions
        [ObservableProperty]
        bool useEpley;

        [ObservableProperty]
        bool useBryzcki;

        [ObservableProperty]
        bool useLombardi;

        [ObservableProperty]
        bool useMayhew;

        [ObservableProperty]
        bool useOConnor;

        [ObservableProperty]
        bool useWathan;


        partial void OnPreferredMassUnitChanged(string value) => _settingsService.MassUnit = UnitsNetHelpers.MassUnitPickerOptions[value];

        partial void OnPreferredLengthUnitChanged(string value) => _settingsService.LengthUnit = UnitsNetHelpers.LengthUnitPickerOptions[value];

        partial void OnUseEpleyChanged(bool value) => _settingsService.UseEpley = value;

        partial void OnUseBryzckiChanged(bool value) => _settingsService.UseBryzcki = value;

        partial void OnUseLombardiChanged(bool value) => _settingsService.UseLombardi = value;

        partial void OnUseMayhewChanged(bool value) => _settingsService.UseMayhew = value;

        partial void OnUseOConnorChanged(bool value) => _settingsService.UseOConnor = value;

        partial void OnUseWathanChanged(bool value) => _settingsService.UseWathan = value;

        #endregion

        [RelayCommand]
        async Task GoTo1RMEquationsOerview() =>
            await Shell.Current.GoToAsync($"{nameof(ORMEquationsOverviewPage)}", animate: true);
        

        [RelayCommand]
        async Task ImportData()
        {
            string action = await Shell.Current.DisplayActionSheet("Import Source", "Cancel", null, "FitNotes: Body Tracker", "FitNotes: Workouts");

            if (action.Equals("FitNotes: Body Tracker"))
            {   
                Loading = true;
                var result = await FilePicker.Default.PickAsync();
        
                if (result is null)
                    await DisplayGenericErrorPrompt("Could not find file");
                else
                {
                    Loading = true;
                    try
                    {
                        var mqs = await _importService.ImportMeasurableQuantitiesAsync(result.FullPath);
                        await _measurableStateService.AddMeasurableQuantities(mqs.Select(mq => new MeasurableQuantityViewModel(mq)));
                        await SnackBarService.ShowSnackBar("Successfully Imported", SnackBarType.Success);
                    }
                    catch
                    {
                        await SnackBarService.ShowSnackBar("Import Failed", SnackBarType.Failure);
                    }
                    Loading = false;
                }
            }

            if (action.Equals("FitNotes: Workouts"))
            {
                Loading = true;
                var result = await FilePicker.Default.PickAsync();

                if (result is null)
                    await DisplayGenericErrorPrompt("Could not find file");
                else
                {
                    Loading = true;
                    try
                    {   
                        await SnackBarService.ShowSnackBar("Successfully Imported", SnackBarType.Success);
                    }
                    catch 
                    {
                        await SnackBarService.ShowSnackBar("Import Failed", SnackBarType.Failure);
                    }
                    Loading = false;
                }
            }
        }

        [RelayCommand]
        async Task DeleteAllData()
        {
            bool confirm = await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete", "Confirm", "Cancel");
            if (confirm) await _context.DeleteAllData();
        }
    }
}

