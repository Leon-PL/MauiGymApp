using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.State;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Utilities;
using MauiGymApp.ViewModels.Workouts.Lifts;
using System.Collections.ObjectModel;

namespace MauiGymApp.ViewModels.Workouts
{
    public partial class AddWorkoutViewModel : BaseViewModel
    {
        private readonly IApplicationStateService _applicationStateService;
        private readonly IRoutinesStateService _routineStateService;
        private readonly IWorkoutStateService _workoutStateService;
        private readonly ILiftsStateService _liftsStateService;

        private WorkoutTemplateViewModel? selectedWorkout { get; set; }

        public AddWorkoutViewModel(IApplicationStateService applicationStateService, IRoutinesStateService routineStateService, IWorkoutStateService workoutStateService, ILiftsStateService liftsStateService)
        {
            _applicationStateService = applicationStateService;
            _routineStateService = routineStateService;
            _workoutStateService = workoutStateService;
            _liftsStateService = liftsStateService;

            OnRoutinesChanged();
            _routineStateService.RoutinesChanged += OnRoutinesChanged;
            LiftsPageViewModel = new LiftsPageViewModel(_applicationStateService, _liftsStateService, _workoutStateService);

            LiftsPageViewModel.LiftsConfirmed += OnLiftsConfirmed;
        }

        private async void OnLiftsConfirmed(IEnumerable<LiftViewModel> selectedLifts)
        {
            if (selectedWorkout is null)
            {
                await _workoutStateService.AddLiftsToWorkout(selectedLifts!.Cast<LiftViewModel>(), _applicationStateService.SelectedDate);
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await _routineStateService.AddLiftsToWorkoutTemplate(SelectedRoutine!, selectedWorkout, selectedLifts!.Cast<LiftViewModel>());
                AllLiftsSelected = false;
                selectedWorkout = null;
            }
        }

        public LiftsPageViewModel LiftsPageViewModel { get; private set; }

        [ObservableProperty]
        List<RoutineViewModel> routines = [];

        void OnRoutinesChanged()
        {
            Routines = _routineStateService.Routines;
            if (Routines.Count > 0)
            {
                SelectedRoutine = Routines.GetLatest();
                AllLiftsSelected = false;
                return;
            }
            AllLiftsSelected = true;
        }

        public ObservableCollection<WorkoutTemplateViewModel>? WorkoutTemplates => SelectedRoutine?.WorkoutTemplates;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(WorkoutTemplates))]
        RoutineViewModel? selectedRoutine;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NotInEditMode))]
        bool inEditMode;

        public bool NotInEditMode => !InEditMode;

        [ObservableProperty]
        bool allLiftsSelected;

        [RelayCommand]
        async Task OpenRoutineSelectionAsync()
        {
            string routine = await Shell.Current.DisplayActionSheet(title: "Select Routine", cancel: null, destruction: null,
                buttons: Routines.Select(r => r.Name).Prepend("Lifts").ToArray());

            if (routine is null) return;
          
                if (routine == "Lifts")
                {
                    AllLiftsSelected = true;
                    return;
                }
                SelectedRoutine = Routines.First(r => r.Name == routine);
                AllLiftsSelected = false;
                
        }

        [RelayCommand]
        async Task AddRoutineAsync()
        {
            string name = await Shell.Current.DisplayPromptAsync(title: "Add Routine", message: "Name", initialValue: "New Routine");
            if (name is null) return;
            RoutineDTO routine = new() { DateCreated=DateTime.Now, Name = name, Notes=""};

            await _routineStateService.AddRoutineAsync(new RoutineViewModel(routine));
            SelectedRoutine = Routines?.Last();
            AllLiftsSelected = false;
        }

        [RelayCommand]
        async Task DeleteRoutineAsync(RoutineViewModel routine)
        {
         
            bool confirm = await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete", "Confirm", "Cancel");
            if (confirm)
            {
                await _routineStateService.DeleteRoutineAsync(routine);
            }
        }

        [RelayCommand]
        async Task SelectWorkoutTemplateAsync(WorkoutTemplateViewModel template)
        {
            await _workoutStateService.ImplementWorkoutTemplate(template, _applicationStateService.SelectedDate);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        void ToggleEditMode() => InEditMode = !InEditMode;

        [RelayCommand]
        void SetAllLiftsSelected(WorkoutTemplateViewModel workout)
        {
            selectedWorkout = workout;
            AllLiftsSelected = true;
        }

        [RelayCommand]
        async Task AddWorkoutTemplate(RoutineViewModel routine)
        {
            var count = SelectedRoutine?.WorkoutTemplates.Count;

            await _routineStateService.AddWorkoutTemplateAsync(routine, new WorkoutTemplateViewModel(new WorkoutTemplateDTO() { Name = $"Day {SelectedRoutine.WorkoutTemplates.Count + 1}" }));
            if (count <= 0)
            {
                InEditMode = true;
            }            
        }

        [RelayCommand]
        void AddSetTemplate(LiftWorkoutTemplateViewModel template)
        {
            var dto = new SetTemplateDTO()
            {
                DateCreated = DateTime.Now,
                LiftWorkoutTemplateId = template.ToModel().Id,
                Notes = "",
                SetNumber = template.SetTemplates.Count + 1,
                PrescribedReps = 0,
                PrescribedRIR = 0,
            };
            template.SetTemplates.Add(new SetTemplateViewModel(dto));
        }

        async Task AddLiftWorkoutTemplate(WorkoutTemplateViewModel workout, IEnumerable<LiftViewModel> lifts)
        {
            foreach (var lift in lifts)
            {
                var dto = new LiftWorkoutTemplateDTO()
                {
                    Lift = lift.ToModel(),
                    DateCreated = DateTime.Now,
                    Notes = "",
                    WorkoutTemplateId = workout.ToModel().Id,
                    SetTemplates = null,
                };

                var template = new LiftWorkoutTemplateViewModel(dto);

                if (workout.LiftWorkoutTemplates.Any(t => t.Lift.ToModel().Id == lift.ToModel().Id))
                {
                    await DisplayGenericErrorPrompt("Lift already in workout");
                    return;
                }
                workout.LiftWorkoutTemplates.Add(template);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            LiftsPageViewModel.LiftsConfirmed -= OnLiftsConfirmed;
        }
    }
}
