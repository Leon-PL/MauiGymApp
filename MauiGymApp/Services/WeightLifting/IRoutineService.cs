using MauiGymApp.Models.DTOs.WeightLifting;
using MauiGymApp.ViewModels.Workouts;
using MauiGymApp.ViewModels.Workouts.Lifts;


namespace MauiGymApp.Services.WeightLifting
{
    public interface IRoutineService
    {
        public event Action<RoutineDTO>? RoutineCreated;
        public event Action<RoutineDTO>? RoutineEdited;
        public event Action<RoutineDTO>? RoutineDeleted;

        Task<IEnumerable<RoutineDTO>> GetAllAsync();
        Task<RoutineDTO> GetAsync(int Id);
        Task AddAsync(RoutineDTO lift);
        Task UpdateAsync(RoutineDTO lift);
        Task DeleteAsync(RoutineDTO lift);
    }
}
