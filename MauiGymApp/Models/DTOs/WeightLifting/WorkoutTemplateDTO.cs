namespace MauiGymApp.Models.DTOs.WeightLifting
{
    public class WorkoutTemplateDTO : BaseDTO
    {
        public string Name { get; set; } = "";  
        public string Notes { get; set; } = "";
        public List<LiftWorkoutTemplateDTO> LiftWorkoutTemplates { get; set; } = [];
        public int RoutineId { get; set; }
    }
}
