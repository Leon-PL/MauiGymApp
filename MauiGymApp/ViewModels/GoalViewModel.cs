using CommunityToolkit.Mvvm.ComponentModel;
using MauiGymApp.Calculations;
using MauiGymApp.Models;
using MauiGymApp.Models.DTOs;
using MauiGymApp.Models.DTOs.Goals;
using MauiGymApp.ViewModels.Common;
using MauiGymApp.ViewModels.Utilities;
using UnitsNet;

namespace MauiGymApp.ViewModels
{
    public partial class GoalViewModel : DTOViewModel<GoalDTO>
    {
        private readonly GoalDTO _goal;

        public GoalViewModel(GoalDTO goal)
        {
            _goal = goal;
            TargetValue = Quantity.From(_goal.TargetValueSI,
                ViewModelHelper.BaseUnitFromQuantityType(goal.QuantityType));
        }

        [ObservableProperty]
        IQuantity targetValue;

        public override GoalDTO ToModel()
        {
            _goal.TargetValueSI = TargetValue.AsBaseUnit();
            return _goal;
        }
    }
}
