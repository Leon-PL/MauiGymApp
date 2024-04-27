namespace MauiGymApp.Models.DTOs.WeightLifting
{
    public class SetTemplateDTO : BaseDTO
    {   
        public int SetNumber { get; set; }
        public int PrescribedReps { get; set; }
        public int PrescribedRIR { get; set; }
        public string Notes { get; set; } = "";
        public int LiftWorkoutTemplateId { get; set; }
    }
}
