using MauiGymApp.Models.DTOs.WeightLifting;

namespace MauiGymApp.Services.WeightLifting
{
    public interface ILiftService
    {
        public event Action<LiftDTO>? LiftCreated;
        public event Action<LiftDTO>? LiftEdited;
        public event Action<LiftDTO>? LiftDeleted;

        Task<IEnumerable<LiftDTO>> GetAllAsync();
        Task<LiftDTO> GetAsync(int Id);
        Task AddAsync(LiftDTO lift);
        Task AddRangeAsync(IEnumerable<LiftDTO> lifts);
        Task UpdateAsync(LiftDTO lift);
        Task DeleteAsync(LiftDTO lift);
    }
}