using MauiGymApp.Models.DTOs;

namespace MauiGymApp.Models.DTOs.WeightLifting
{
    public sealed class SetDTO : BaseDTO
    {
        public double WeightKg { get; set; }
        public int Reps { get; set; }
        public int? RIR { get; set; }
        public int LiftWorkoutId { get; set; }
    }
}
