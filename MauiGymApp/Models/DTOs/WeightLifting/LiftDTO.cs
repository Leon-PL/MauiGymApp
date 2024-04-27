using MauiGymApp.Models.DTOs.Goals;

namespace MauiGymApp.Models.DTOs.WeightLifting
{
    public sealed class LiftDTO : BaseDTO
    {
        public string Name { get; set; } = "";
        public MovementPattern MovementPattern { get; set; }

        public GoalDTO? E1RMGoal { get; set; }
    }
}
