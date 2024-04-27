using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Utilities;
using System.Collections.ObjectModel;
using UnitsNet;
using UnitsNet.GenericMath;

namespace MauiGymApp.ViewModels.Workouts.Lifts
{
    public partial class LiftWorkoutViewModel : DTOViewModel<LiftWorkoutDTO>
    {
        private readonly LiftWorkoutDTO _liftWorkout;

        public LiftWorkoutViewModel(LiftWorkoutDTO liftWorkout)
        {
            _liftWorkout = liftWorkout;

            Lift = new LiftViewModel(_liftWorkout.Lift);
            Sets = new ObservableCollection<SetViewModel>(_liftWorkout.Sets.Select(s => new SetViewModel(s)));
            DateTime = _liftWorkout.DateTime;
        }

        [ObservableProperty]
        LiftViewModel lift;

        [ObservableProperty]
        ObservableCollection<SetViewModel> sets;

        [ObservableProperty]
        DateTime dateTime;

        [ObservableProperty]
        bool showSets;

        [RelayCommand]
        public void ToggleShowSets() => ShowSets = !ShowSets;

        [RelayCommand]
        void RemoveSet(SetViewModel set) => Sets.Remove(set);

        public Mass Tonnage => Sets.Select(s => s.Tonnage).Sum();

        public override LiftWorkoutDTO ToModel()
        {
            _liftWorkout.Lift = Lift.ToModel();
            _liftWorkout.Sets = Sets.ToModels().ToList();
            _liftWorkout.DateTime = DateTime;

            return _liftWorkout;
        }
    }
}
