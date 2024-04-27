using MauiGymApp.Models.DTOs.Goals;

namespace MauiGymApp.Models.DTOs.Measurables
{
    public sealed class MeasurableQuantityDTO : BaseDTO
    {
        public string Name { get; set; } = "";
        public QuantityType QuantityType { get; set; }
        public string Notes { get; set; } = "";
        public List<MeasurementDTO> Measurements { get; } = [];

        public GoalDTO? Goal { get; set; }
    }
}
