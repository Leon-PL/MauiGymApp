using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Interfaces;

namespace MauiGymApp.ViewModels.Workouts
{
    public partial class SetTemplateViewModel : DTOViewModel<SetTemplateDTO>, ITemplate<SetViewModel>
    {
        private readonly SetTemplateDTO _template;

        public SetTemplateViewModel(SetTemplateDTO template)
        {
            _template = template;

            SetNumber = _template.SetNumber;
            PrescribedReps = _template.PrescribedReps;
            PrescribedRIR = _template.PrescribedRIR;
            Notes = _template.Notes;
        }

        public string Name => $"Set {SetNumber}";

        [ObservableProperty]
        int setNumber;

        [ObservableProperty]
        int prescribedReps;

        [ObservableProperty]
        int prescribedRIR;

        [ObservableProperty]
        string notes;

        [RelayCommand]
        void IncrementReps() => PrescribedReps += 1;

        [RelayCommand]
        void DecrementReps()
        {   
            if (PrescribedReps > 0) PrescribedReps -= 1;
        }
         
        [RelayCommand]
        void IncrementRIR() => PrescribedRIR += 1;

        [RelayCommand]
        void DecrementRIR()
        {
            if (PrescribedRIR > 0) PrescribedRIR -= 1;
        }

        public SetViewModel ToImplementation()
        {
            SetDTO dto = new()
            {
                DateCreated = DateTime.Now,
                WeightKg = 0,
                Reps = PrescribedReps,
                RIR = PrescribedRIR,
                LiftWorkoutId = _template.LiftWorkoutTemplateId,
            };

            return new SetViewModel(dto);
        }

        public override SetTemplateDTO ToModel()
        {
            _template.SetNumber = SetNumber;
            _template.PrescribedReps = PrescribedReps;
            _template.PrescribedRIR = PrescribedRIR;

            return _template;
        }
    }
}
