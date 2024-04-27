using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.Nito;
using MauiGymApp.Services.WeightLifting;
using MauiGymApp.ViewModels.Workouts;
using MauiGymApp.ViewModels.Workouts.Lifts;

namespace MauiGymApp.State
{
    public class WorkoutStateService : IWorkoutStateService
    {
        private readonly IWorkoutService _workoutService;
        private readonly NotifyTask<IEnumerable<WorkoutViewModel>> LoadWorkoutsTask;

        public List<WorkoutViewModel> Workouts { get; private set; } = [];
        public event Action? WorkoutsChanged;

        public WorkoutStateService(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
            LoadWorkoutsTask = NotifyTask.Create(Load, []);
        }

        public async Task<IEnumerable<WorkoutViewModel>> Load()
        {
            var result = await _workoutService.GetAllAsync();
            Workouts = result.Select(w => new WorkoutViewModel(w)).ToList();
            
            WorkoutsChanged?.Invoke();
            return Workouts;
        }

        public async Task ImplementWorkoutTemplate(WorkoutTemplateViewModel template, DateTime date)
        {
            var existingWorkout = Workouts.Find(w => w.DateTime.Date == date.Date);
            var implementation = template.ToImplementation();
            if (existingWorkout is null)
            {
                await AddWorkout(implementation);
            } 
            else
            {
                existingWorkout.Merge(implementation);
                await UpdateWorkout(existingWorkout);
            }
        }

        #region CRUD
        public async Task AddWorkout(WorkoutViewModel measurableQuantity)
        {
            await _workoutService.AddAsync(measurableQuantity.ToModel());
            Workouts.Add(measurableQuantity);
            WorkoutsChanged?.Invoke();
        }
        
        public async Task AddWorkouts(IEnumerable<WorkoutViewModel> workouts)
        {
            await _workoutService.AddRangeAsync(workouts.Select(w => w.ToModel()));
            WorkoutsChanged?.Invoke();
        }

        public async Task DeleteWorkout(WorkoutViewModel measurableQuantity)
        {
            await _workoutService.DeleteAsync(measurableQuantity.ToModel());
            Workouts.Remove(measurableQuantity);
            WorkoutsChanged?.Invoke();
        }

        public async Task UpdateWorkout(WorkoutViewModel measurableQuantity)
        {
            await _workoutService.UpdateAsync(measurableQuantity.ToModel());
            WorkoutsChanged?.Invoke();
        }

        public async Task DeleteLiftWorkout(WorkoutViewModel workout, LiftWorkoutViewModel liftWorkout)
        {
            workout.LiftWorkouts.Remove(liftWorkout);
            await UpdateWorkout(workout);
        }

        #endregion

        public async Task AddLiftsToWorkout(IEnumerable<LiftViewModel> lifts, DateTime date)
        {
            var workout = Workouts.Find(w => w.DateTime.Date == date.Date);

            var lwdtos = lifts.Select(l => new LiftWorkoutDTO()
            {
                Lift = l.ToModel(),
                DateTime = date
            });


            if (workout is null)
            {
                var wdto = new WorkoutDTO
                {
                    LiftWorkouts = lwdtos.ToList()
                };
                var newWorkout = new WorkoutViewModel(wdto);

                await AddWorkout(newWorkout);
            }
            else
            {
                lwdtos.ToList().ForEach(lw => workout.LiftWorkouts.Add(new LiftWorkoutViewModel(lw)));
                await UpdateWorkout(workout);
            }
        }
    }
}
