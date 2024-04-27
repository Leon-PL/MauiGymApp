using MauiGymApp.Models.DTOs.WeightLifting;

namespace MauiGymApp.Services.WeightLifting
{
    public interface IWorkoutService
    {
        public event Action<WorkoutDTO>? WorkoutCreated;
        public event Action<IEnumerable<WorkoutDTO>>? WorkoutsCreated;
        public event Action<WorkoutDTO>? WorkoutEdited;
        public event Action<WorkoutDTO>? WorkoutDeleted;

        public event Action<WorkoutTemplateDTO>? TemplateCreated;
        public event Action<WorkoutTemplateDTO>? TemplateEdited;
        public event Action<WorkoutTemplateDTO>? TemplateDeleted;

        Task<IEnumerable<WorkoutDTO>> GetAllAsync();
        Task<WorkoutDTO> GetAsync(int Id);
        Task AddAsync(WorkoutDTO workout);
        Task AddRangeAsync(IEnumerable<WorkoutDTO> workouts);
        Task UpdateAsync(WorkoutDTO workout);
        Task DeleteAsync(WorkoutDTO workout);

        Task<IEnumerable<WorkoutTemplateDTO>> GetAllTemplatesAsync();
        Task<WorkoutTemplateDTO> GetTemplateAsync(int Id);
        Task AddTemplateAsync(WorkoutTemplateDTO workout);
        Task UpdateTemplateAsync(WorkoutTemplateDTO workout);
        Task DeleteTemplateAsync(WorkoutTemplateDTO workout);
    }
}
