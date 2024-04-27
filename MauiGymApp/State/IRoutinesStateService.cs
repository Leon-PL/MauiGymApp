using MauiGymApp.ViewModels.Workouts;
using MauiGymApp.ViewModels.Workouts.Lifts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiGymApp.State
{
    public interface IRoutinesStateService
    {
        public event Action? RoutinesChanged;

        public bool Loaded { get; }
        public List<RoutineViewModel> Routines { get; }

        #region CRUD
        Task AddRoutineAsync(RoutineViewModel routine);
        Task DeleteRoutineAsync(RoutineViewModel routine);

        Task AddWorkoutTemplateAsync(RoutineViewModel routine, WorkoutTemplateViewModel template);
        #endregion

        Task AddLiftsToWorkoutTemplate(RoutineViewModel routine, WorkoutTemplateViewModel template, IEnumerable<LiftViewModel> lifts);
    }
}
