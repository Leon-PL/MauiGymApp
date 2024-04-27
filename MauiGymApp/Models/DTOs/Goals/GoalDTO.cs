namespace MauiGymApp.Models.DTOs.Goals
{
    public class GoalDTO : BaseDTO
    {
        /// <summary>
        /// Value stored in SI units.
        /// </summary>
        public double TargetValueSI { get; set; }
        public QuantityType QuantityType { get; set; }
    }
}
