using MauiGymApp.Contexts;
using MauiGymApp.Models.DTOs.WeightLifting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MauiGymApp.Services.WeightLifting
{
    public class LiftWorkoutService : ILiftWorkoutService
    {
        private readonly MainContext _context;

        public event Action<LiftWorkoutDTO>? LiftWorkoutCreated;
        public event Action<LiftWorkoutDTO>? LiftWorkoutEdited;
        public event Action<LiftWorkoutDTO>? LiftWorkoutDeleted;

        public event Action<LiftWorkoutTemplateDTO>? TemplateCreated;
        public event Action<LiftWorkoutTemplateDTO>? TemplateEdited;
        public event Action<LiftWorkoutTemplateDTO>? TemplateDeleted;

        public LiftWorkoutService(MainContext context)
        {
            _context = context;
        }

        protected IIncludableQueryable<LiftWorkoutDTO, List<SetDTO>> EagerLoad()
            => _context.LiftWorkouts.Include(lw => lw.Lift)
                                    .Include(lw => lw.Sets);

        public async Task<IEnumerable<LiftWorkoutDTO>> GetAllAsync()
            => await EagerLoad().ToListAsync();

        public async Task<LiftWorkoutDTO> GetAsync(int Id)
            => await EagerLoad().SingleAsync(w => w.Id == Id);

        public async Task AddAsync(LiftWorkoutDTO LiftWorkout)
        {
            await _context.AddAsync(LiftWorkout);
            await _context.SaveChangesAsync();
            LiftWorkoutCreated?.Invoke(LiftWorkout);
        }

        public async Task UpdateAsync(LiftWorkoutDTO LiftWorkout)
        {
            _context.Update(LiftWorkout);
            await _context.SaveChangesAsync();
            LiftWorkoutEdited?.Invoke(LiftWorkout);
        }

        public async Task DeleteAsync(LiftWorkoutDTO LiftWorkout)
        {
            _context.Remove(LiftWorkout);
            await _context.SaveChangesAsync();
            LiftWorkoutDeleted?.Invoke(LiftWorkout);
        }

        public async Task<IEnumerable<LiftWorkoutDTO>> GetLiftWorkouts(LiftDTO lift)
           => await EagerLoad().Where(l => l.Id == lift.Id).ToListAsync();

        protected IIncludableQueryable<LiftWorkoutTemplateDTO, LiftDTO> EagerLoadTemplates()
            => _context.LiftWorkoutTemplates.Include(lwt => lwt.Lift);

        public async Task<IEnumerable<LiftWorkoutTemplateDTO>> GetAllTemplatesAsync()
            => await EagerLoadTemplates().ToListAsync();

        public async Task<LiftWorkoutTemplateDTO> GetTemplateAsync(int Id)
            => await EagerLoadTemplates().SingleAsync(w => w.Id == Id);

        public async Task AddTemplateAsync(LiftWorkoutTemplateDTO template)
        {
            await _context.AddAsync(template);
            await _context.SaveChangesAsync();
            TemplateCreated?.Invoke(template);
        }

        public async Task UpdateTemplateAsync(LiftWorkoutTemplateDTO template)
        {
            _context.Update(template);
            await _context.SaveChangesAsync();
            TemplateEdited?.Invoke(template);
        }

        public async Task DeleteTemplateAsync(LiftWorkoutTemplateDTO template)
        {
            _context.Remove(template);
            await _context.SaveChangesAsync();
            TemplateDeleted?.Invoke(template);
        }
    }
}
