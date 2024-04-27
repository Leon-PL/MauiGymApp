using MauiGymApp.Models.DTOs.WeightLifting;

namespace MauiGymApp.Services.WeightLifting
{
    public interface ILiftWorkoutService
    {
        public event Action<LiftWorkoutDTO>? LiftWorkoutCreated;
        public event Action<LiftWorkoutDTO>? LiftWorkoutEdited;
        public event Action<LiftWorkoutDTO>? LiftWorkoutDeleted;

        public event Action<LiftWorkoutTemplateDTO>? TemplateCreated;
        public event Action<LiftWorkoutTemplateDTO>? TemplateEdited;
        public event Action<LiftWorkoutTemplateDTO>? TemplateDeleted;

        Task<IEnumerable<LiftWorkoutDTO>> GetAllAsync();
        Task<LiftWorkoutDTO> GetAsync(int Id);
        Task AddAsync(LiftWorkoutDTO liftWorkout);
        Task UpdateAsync(LiftWorkoutDTO liftWorkout);
        Task DeleteAsync(LiftWorkoutDTO liftWorkout);

        Task<IEnumerable<LiftWorkoutDTO>> GetLiftWorkouts(LiftDTO lift);

        Task<IEnumerable<LiftWorkoutTemplateDTO>> GetAllTemplatesAsync();
        Task<LiftWorkoutTemplateDTO> GetTemplateAsync(int Id);
        Task AddTemplateAsync(LiftWorkoutTemplateDTO template);
        Task UpdateTemplateAsync(LiftWorkoutTemplateDTO template);
        Task DeleteTemplateAsync(LiftWorkoutTemplateDTO template);
    }
}
