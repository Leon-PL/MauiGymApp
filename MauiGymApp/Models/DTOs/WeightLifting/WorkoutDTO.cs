using MauiGymApp.Models.DTOs;

namespace MauiGymApp.Models.DTOs.WeightLifting
{
    public sealed class WorkoutDTO : BaseDTO
    {
        public string Name { get; set; } = "";
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Notes { get; set; } = "";
        public List<LiftWorkoutDTO> LiftWorkouts { get; set; } = [];
        public int RoutineId { get; set; }
    }
}
