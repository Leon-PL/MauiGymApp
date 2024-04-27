using MauiGymApp.ViewModels.Workouts;
using MauiGymApp.ViewModels.Workouts.Lifts;

namespace MauiGymApp.State
{
    public interface IWorkoutStateService
    {
        public event Action? WorkoutsChanged;
        public List<WorkoutViewModel> Workouts { get; }
        public Task AddLiftsToWorkout(IEnumerable<LiftViewModel> lifts, DateTime date);

        /// <summary>
        /// Implements a workout template on a given day, will merge workouts if workout already 
        /// exists on the same day.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="date"></param>
        public Task ImplementWorkoutTemplate(WorkoutTemplateViewModel template, DateTime date);

        Task AddWorkout(WorkoutViewModel workout);
        Task AddWorkouts(IEnumerable<WorkoutViewModel> workouts);
        Task DeleteWorkout(WorkoutViewModel workout);
        Task UpdateWorkout(WorkoutViewModel workout);

        Task DeleteLiftWorkout(WorkoutViewModel workout, LiftWorkoutViewModel liftWorkout);
    }
}
