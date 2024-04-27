using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.Nito;
using MauiGymApp.Services.WeightLifting;
using MauiGymApp.ViewModels.Workouts;
using MauiGymApp.ViewModels.Workouts.Lifts;

namespace MauiGymApp.State
{
    public class RoutineStateService : IRoutinesStateService
    {
        private readonly IRoutineService _routineService;

        private IEnumerable<RoutineDTO> _routines { get; set; } = [];
        private readonly NotifyTask<IEnumerable<RoutineViewModel>> LoadRoutinesTask;

        public event Action? RoutinesChanged;
        public bool Loaded => LoadRoutinesTask.IsSuccessfullyCompleted;
        public List<RoutineViewModel> Routines { get; private set; } = [];

        public RoutineStateService(IRoutineService routineService)
        {
            _routineService = routineService;
            LoadRoutinesTask = NotifyTask.Create(Load, []);
        }

        public async Task<IEnumerable<RoutineViewModel>> Load()
        {
            _routines = await _routineService.GetAllAsync();
            Routines = _routines.Select(r => new RoutineViewModel(r)).ToList();
            RoutinesChanged?.Invoke();
            return Routines;
        }

        public async Task AddRoutineAsync(RoutineViewModel routine)
        {
            await _routineService.AddAsync(routine.ToModel());
            Routines.Add(routine);
            RoutinesChanged?.Invoke();
        }

        public async Task UpdateRoutineAsync(RoutineViewModel routine)
        {
            await _routineService.UpdateAsync(routine.ToModel());
            RoutinesChanged?.Invoke();
        }

        public async Task DeleteRoutineAsync(RoutineViewModel routine)
        {
            await _routineService.DeleteAsync(routine.ToModel());
            Routines.Remove(routine);
            RoutinesChanged?.Invoke();
        }

        public async Task AddWorkoutTemplateAsync(RoutineViewModel routine, WorkoutTemplateViewModel template)
        {
            routine.WorkoutTemplates.Add(template);
            await UpdateRoutineAsync(routine);
        }

        public async Task AddLiftsToWorkoutTemplate(RoutineViewModel routine,  WorkoutTemplateViewModel template, IEnumerable<LiftViewModel> lifts)
        {
            var lwdtos = lifts.Select(l => new LiftWorkoutTemplateDTO()
            { 
                Lift = l.ToModel(),
            });

            lwdtos.ToList().ForEach(lw => template.LiftWorkoutTemplates.Add(new LiftWorkoutTemplateViewModel(lw)));
            await UpdateRoutineAsync(routine);
        }
    }
}
