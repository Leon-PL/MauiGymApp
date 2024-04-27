using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.Nito;
using MauiGymApp.Services.Visual;
using MauiGymApp.Services.WeightLifting;
using MauiGymApp.State;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Workouts;
using MauiGymApp.ViewModels.Workouts.Lifts;
using UnitsNet;

namespace MauiGymApp.ViewModels.Home
{
    public partial class HomePageWorkoutViewModel : BaseViewModel
    {   
        private readonly IWorkoutStateService _workoutStateService;
        private readonly IWorkoutService _workoutService;

        public HomePageWorkoutViewModel(IWorkoutService workoutService, IWorkoutStateService workoutStateService)
        {
            _workoutStateService = workoutStateService;
            _workoutService = workoutService;
            selectedDate = DateTime.Now;

            OnSourceWorkoutsChanged();
            _workoutStateService.WorkoutsChanged += OnSourceWorkoutsChanged;
        }

        public void OnSourceWorkoutsChanged()
        {
            var workouts =  _workoutStateService.Workouts;
            
            var result = new Dictionary<DateTime, WorkoutViewModel>();
            workouts.ForEach(vm => result[vm.DateTime.Date] = vm);
            Workouts = result;        
         }

   
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedWorkout))]
        Dictionary<DateTime, WorkoutViewModel> workouts = [];

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedWorkout))]
        DateTime selectedDate;

        public WorkoutViewModel? SelectedWorkout
        {
            get
            {
                Workouts.TryGetValue(SelectedDate.Date, out WorkoutViewModel? result);
                return result;
            }
        }

        [RelayCommand]
        async Task OpenMoreOptions(object obj)
        {   

            var action = await Shell.Current.DisplayActionSheet("Options", "Cancel", null, "View", "Delete");
            if (action is null || obj is null) return;

            var result = (List<object>)obj;
            var w = (WorkoutViewModel)result[0];
            var lw = (LiftWorkoutViewModel)result[1];

            if (action == "View")
            {
                await GoToLiftAsync(lw.Lift);
            }

            if (action == "Delete")
            { 
                await _workoutStateService.DeleteLiftWorkout(w, lw);
                await SnackBarService.ShowSnackBar($"{lw.Lift.Name} removed from workout", SnackBarType.Success);
             }
        }

        [RelayCommand]
        async Task AddSetAsync(LiftWorkoutViewModel liftWorkout)
        {
            if (liftWorkout is null) await DisplayGenericErrorPrompt();
            SetViewModel newSet;
            if (liftWorkout.Sets.Count == 0) newSet = new SetViewModel(new SetDTO() { WeightKg = 0, Reps = 0 });
            else
            {
                var prevSet = liftWorkout.Sets.Last().ToModel();
                var newDTO = new SetDTO()
                {
                    DateCreated = DateTime.Now,
                    WeightKg = prevSet.WeightKg,
                    Reps = prevSet.Reps,
                    RIR = prevSet.RIR,
                    LiftWorkoutId = prevSet.LiftWorkoutId,
                };
                newSet = new SetViewModel(newDTO);
            }
            liftWorkout.Sets.Add(newSet);
            liftWorkout.ShowSets = true;
        }

        public RoutineViewModel? Routine;

        [RelayCommand]
        void IncrementReps(SetViewModel set) => set.Reps += 1;

        [RelayCommand]
        void DecrementReps(SetViewModel set) => set.Reps -= 1;

        [RelayCommand]
        void IncrementWeight(SetViewModel set) => set.Weight += Mass.FromKilograms(5);

        [RelayCommand]
        void DecrementWeight(SetViewModel set) => set.Weight -= Mass.FromKilograms(5);

        [RelayCommand]
        void IncrementRIR(SetViewModel set)
        {
            if (set.RepsInReserve is null || set.RepsInReserve == 10) return;
            set.RepsInReserve += 1;
        }

        [RelayCommand]
        void DecrementRIR(SetViewModel set)
        {
            if (set.RepsInReserve is null || set.RepsInReserve == 0) return;
            set.RepsInReserve -= 1;
        }

        [RelayCommand]
        async Task DeleteWorkoutAsync() => await _workoutService.DeleteAsync(SelectedWorkout.ToModel());

        public override void Dispose()
        {
            base.Dispose();

            _workoutStateService.WorkoutsChanged -= OnSourceWorkoutsChanged;
        }
    }
}
