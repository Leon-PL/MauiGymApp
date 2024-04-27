using MauiGymApp.Contexts;
using MauiGymApp.Models.DTOs.WeightLifting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MauiGymApp.Services.WeightLifting
{
    public class WorkoutService : IWorkoutService
    {
        private readonly MainContext _context;

        public event Action<WorkoutDTO>? WorkoutCreated;
        public event Action<IEnumerable<WorkoutDTO>>? WorkoutsCreated;
        public event Action<WorkoutDTO>? WorkoutEdited;
        public event Action<WorkoutDTO>? WorkoutDeleted;

        public event Action<WorkoutTemplateDTO>? TemplateCreated;
        public event Action<WorkoutTemplateDTO>? TemplateEdited;
        public event Action<WorkoutTemplateDTO>? TemplateDeleted;

        public WorkoutService(MainContext context)
        {
            _context = context;
        }

        protected IIncludableQueryable<WorkoutDTO, List<SetDTO>> EagerLoad()
            => _context.Workouts.Include(w => w.LiftWorkouts)
                                .ThenInclude(lw => lw.Lift)
                                .Include(r => r.LiftWorkouts)
                                .ThenInclude(lw => lw.Sets);

        public async Task<IEnumerable<WorkoutDTO>> GetAllAsync()
            => await EagerLoad().ToListAsync();

        public async Task<WorkoutDTO> GetAsync(int Id)
            => await EagerLoad().SingleAsync(w => w.Id == Id);

        public async Task AddAsync(WorkoutDTO workout)
        {
            await _context.AddAsync(workout);
            await _context.SaveChangesAsync();
            WorkoutCreated?.Invoke(workout);
        }

        public async Task AddRangeAsync(IEnumerable<WorkoutDTO> workouts)
        {
            await _context.AddRangeAsync(workouts);
            await _context.SaveChangesAsync();
            WorkoutsCreated?.Invoke(workouts);
        }

        public async Task UpdateAsync(WorkoutDTO workout)
        {
            _context.Update(workout);
            await _context.SaveChangesAsync();
            WorkoutEdited?.Invoke(workout);
        }

        public async Task DeleteAsync(WorkoutDTO workout)
        {
            _context.Remove(workout);
            await _context.SaveChangesAsync();
            WorkoutDeleted?.Invoke(workout);
        }

        protected IIncludableQueryable<WorkoutTemplateDTO, LiftDTO> EagerLoadTemplates()
            => _context.WorkoutTemplates.Include(wt => wt.LiftWorkoutTemplates).ThenInclude(lwt => lwt.Lift);

        public async Task<IEnumerable<WorkoutTemplateDTO>> GetAllTemplatesAsync()
            => await EagerLoadTemplates().ToListAsync();

        public async Task<WorkoutTemplateDTO> GetTemplateAsync(int Id)
            => await EagerLoadTemplates().SingleAsync(w => w.Id == Id);

        public async Task AddTemplateAsync(WorkoutTemplateDTO template)
        {
            await _context.AddAsync(template);
            await _context.SaveChangesAsync();
            TemplateCreated?.Invoke(template);
        }

        public async Task UpdateTemplateAsync(WorkoutTemplateDTO template)
        {
            _context.Update(template);
            await _context.SaveChangesAsync();
            TemplateEdited?.Invoke(template);
        }

        public async Task DeleteTemplateAsync(WorkoutTemplateDTO template)
        {
            _context.Remove(template);
            await _context.SaveChangesAsync();
            TemplateDeleted?.Invoke(template);
        }

    }
}
