namespace MauiGymApp.Models.DTOs.WeightLifting
{
    public sealed class LiftWorkoutDTO : BaseDTO
    {
        public LiftDTO Lift { get; set; }
        public List<SetDTO> Sets { get; set; } = [];
        public DateTime DateTime { get; set; }
        public int WorkoutId { get; set; }
    }
}
