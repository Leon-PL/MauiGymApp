using CommunityToolkit.Mvvm.ComponentModel;
using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Utilities;
using System.Collections.ObjectModel;

namespace MauiGymApp.ViewModels.Workouts
{
    public partial class RoutineViewModel : DTOViewModel<RoutineDTO>
    {
        private readonly RoutineDTO _routine;

        public RoutineViewModel(RoutineDTO routine)
        {
            _routine = routine;

            Name = _routine.Name;
            Workouts = new ObservableCollection<WorkoutViewModel>(_routine.Workouts.Select(w => new WorkoutViewModel(w)));
            WorkoutTemplates = new ObservableCollection<WorkoutTemplateViewModel>(_routine.WorkoutTemplates.Select(wt => new WorkoutTemplateViewModel(wt)));
            Notes = _routine.Notes;
        }

        [ObservableProperty]
        string name;

        [ObservableProperty]
        ObservableCollection<WorkoutViewModel> workouts;

        [ObservableProperty]
        ObservableCollection<WorkoutTemplateViewModel> workoutTemplates;

        [ObservableProperty]
        string notes;

        public override RoutineDTO ToModel()
        {
            _routine.Name = Name;
            _routine.Workouts = Workouts.ToModels().ToList();
            _routine.WorkoutTemplates = WorkoutTemplates.ToModels().ToList();
            _routine.Notes = Notes;

            return _routine;
        }

        public static IEnumerable<WorkoutTemplateViewModel> FakeData()
        {
            var setdtos = new List<SetTemplateDTO>()
            {
                new()
                {   
                    SetNumber = 1,
                    PrescribedReps = 10,
                    PrescribedRIR = 1,
                    Notes="",
                },
                new()
                {   
                    SetNumber = 2,
                    PrescribedReps = 10,
                    PrescribedRIR = 1,
                    Notes="",
                },
                new()
                {   
                    SetNumber = 3,
                    PrescribedReps = 10,
                    PrescribedRIR = 1,
                    Notes="",
                },
            };

            var lwdtos = new List<LiftWorkoutTemplateDTO>()
            {
                new()
                {
                    Lift = new LiftDTO(){Name = "Hack Squat"},
                    SetTemplates = setdtos,
                    Notes="",
                }
            };

            return new List<WorkoutTemplateDTO>()
            {
                new()
                {   
                    LiftWorkoutTemplates = lwdtos,
                    Name="Test Workout",
                    Notes="This workout is for development purposes",
                }
            }.Select(w => new WorkoutTemplateViewModel(w));
        }
    }
}
