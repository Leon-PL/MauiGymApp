using CommunityToolkit.Mvvm.ComponentModel;
using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.ViewModels.Common;
using UnitsNet;

namespace MauiGymApp.ViewModels.Workouts
{
    public partial class SetViewModel : DTOViewModel<SetDTO>
    {
        private readonly SetDTO _set;

        public SetViewModel(SetDTO set)
        {
            _set = set;

            Weight = Mass.FromKilograms(_set.WeightKg);
            Reps = _set.Reps;
            RepsInReserve = _set.RIR;
        }

        [ObservableProperty]
        Mass weight;        

        [ObservableProperty]
        int reps;

        [ObservableProperty]
        int? repsInReserve;

        public Mass Tonnage => Weight * Reps;

        public override SetDTO ToModel()
        {
            _set.WeightKg = Weight.Kilograms;
            _set.Reps = Reps;
            _set.RIR = RepsInReserve;

            return _set;
        }
    }
}