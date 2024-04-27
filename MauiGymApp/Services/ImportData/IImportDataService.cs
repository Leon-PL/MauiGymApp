using MauiGymApp.Models.DTOs.Measurables;
using MauiGymApp.Models.DTOs.WeightLifting;

namespace MauiGymApp.Services.ImportData
{
    public interface IImportDataService
    {
        Task<IEnumerable<MeasurableQuantityDTO>> ImportMeasurableQuantitiesAsync(string dataPath);
        Task<IEnumerable<WorkoutDTO>> ImportWorkoutsAsync(string dataPath);
    }
}
