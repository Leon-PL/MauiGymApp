namespace MauiGymApp.Models.DTOs.WeightLifting
{
    public class LiftWorkoutTemplateDTO : BaseDTO
    {
        public LiftDTO Lift { get; set; }
        public List<SetTemplateDTO>? SetTemplates { get; set; } = [];
        public string Notes { get; set; } = "";
        public int WorkoutTemplateId { get; set; }
    }
}
