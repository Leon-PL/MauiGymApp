using CommunityToolkit.Mvvm.ComponentModel;
using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Interfaces;
using MauiGymApp.ViewModels.Utilities;
using MauiGymApp.ViewModels.Workouts.Lifts;
using System.Collections.ObjectModel;

namespace MauiGymApp.ViewModels.Workouts
{
    public partial class WorkoutTemplateViewModel : DTOViewModel<WorkoutTemplateDTO>, ITemplate<WorkoutViewModel>
    {
        private readonly WorkoutTemplateDTO _template;

        public WorkoutTemplateViewModel(WorkoutTemplateDTO template)
        {
            _template = template;

            Name = _template.Name;
            Notes = _template.Notes;
            LiftWorkoutTemplates = new ObservableCollection<LiftWorkoutTemplateViewModel>(_template.LiftWorkoutTemplates.Select(lwt => new LiftWorkoutTemplateViewModel(lwt)));
        }

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string notes;

        [ObservableProperty]
        ObservableCollection<LiftWorkoutTemplateViewModel> liftWorkoutTemplates;

        public WorkoutViewModel ToImplementation()
        {
            WorkoutDTO dto = new()
            {
                Name = Name,

                Notes = Notes,
                DateCreated = DateTime.Now,
                DateTime = DateTime.Now,
                LiftWorkouts = LiftWorkoutTemplates.ToImplementations().ToModels().ToList(),
                RoutineId = _template.RoutineId,
            };

            return new WorkoutViewModel(dto);
        }

        public WorkoutViewModel ToImplementation(DateTime date)
        {
            WorkoutDTO dto = new()
            {
                Name = Name,

                Notes = Notes,
                DateCreated = DateTime.Now,
                DateTime = date,
                LiftWorkouts = LiftWorkoutTemplates.ToImplementations().ToModels().ToList(),
                RoutineId = _template.RoutineId,
            };

            return new WorkoutViewModel(dto);
        }

        public override WorkoutTemplateDTO ToModel()
        {
            _template.Name = Name;
            _template.Notes = Notes;
            _template.LiftWorkoutTemplates = LiftWorkoutTemplates.ToModels().ToList();

            return _template;
        }
    }
}
