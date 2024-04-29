namespace MauiGymApp.Models.DTOs.Measurables
{
    public sealed class MeasurementDTO : BaseDTO
    {
        public double ValueSI { get; set; }
        public QuantityType QuantityType { get; set; }
        public DateTime DateTime { get; set; }
        public byte[] Image { get; set; } = [];
        public int MeasurableQuantityDTOID { get; set; }
    }
}
