using MauiGymApp.Models.DTOs.Measurables;

namespace MauiGymApp.Services.Measurables
{
    public interface IMeasurablesService
    {
        Task<IEnumerable<MeasurableQuantityDTO>> GetAllAsync();
        Task<MeasurableQuantityDTO> GetAsync(int Id);
        Task AddAsync(MeasurableQuantityDTO quantity);
        Task AddRangeAsync(IEnumerable<MeasurableQuantityDTO> quantity);
        Task UpdateAsync(MeasurableQuantityDTO quantity);
        Task DeleteAsync(MeasurableQuantityDTO quantity);

        Task AddMeasurementAsync(MeasurableQuantityDTO quantity, MeasurementDTO measurement);
        Task UpdateMeasurementAsync(MeasurableQuantityDTO quantity, MeasurementDTO measurement);
        Task DeleteMeasurementAsync(MeasurableQuantityDTO quantity, MeasurementDTO measurement);
    }
}
