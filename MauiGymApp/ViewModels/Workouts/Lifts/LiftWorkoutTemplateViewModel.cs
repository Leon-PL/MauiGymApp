using CommunityToolkit.Mvvm.ComponentModel;
using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Interfaces;
using MauiGymApp.ViewModels.Utilities;
using System.Collections.ObjectModel;

namespace MauiGymApp.ViewModels.Workouts.Lifts
{
    public partial class LiftWorkoutTemplateViewModel : DTOViewModel<LiftWorkoutTemplateDTO>, ITemplate<LiftWorkoutViewModel>
    {
        private readonly LiftWorkoutTemplateDTO _template;

        public LiftWorkoutTemplateViewModel(LiftWorkoutTemplateDTO template)
        {
            _template = template;

            Lift = new LiftViewModel(_template.Lift);
            SetTemplates = _template.SetTemplates is null ? new ObservableCollection<SetTemplateViewModel>()
                : new ObservableCollection<SetTemplateViewModel>(_template.SetTemplates.Select(st => new SetTemplateViewModel(st)));
            Notes = _template.Notes;
        }

        [ObservableProperty]
        LiftViewModel lift;

        [ObservableProperty]
        ObservableCollection<SetTemplateViewModel> setTemplates;

        [ObservableProperty]
        string notes;

        public LiftWorkoutViewModel ToImplementation()
        {
            LiftWorkoutDTO dto = new()
            {
                DateCreated = DateTime.Now,
                DateTime = DateTime.Now,
                Lift = Lift.ToModel(),
                Sets = SetTemplates.ToImplementations().ToModels().ToList(),
                WorkoutId = _template.WorkoutTemplateId,
            };

            return new LiftWorkoutViewModel(dto);
        }

        public override LiftWorkoutTemplateDTO ToModel()
        {
            _template.Lift = Lift.ToModel();
            _template.SetTemplates = SetTemplates.ToModels().ToList();
            _template.Notes = Notes;

            return _template;

        }
    }
}
