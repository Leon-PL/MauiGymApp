using MauiGymApp.Contexts;
using MauiGymApp.Models.DTOs.WeightLifting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MauiGymApp.Services.WeightLifting
{
    public class RoutineService : IRoutineService
    {
        private readonly MainContext _context;

        public event Action<RoutineDTO>? RoutineCreated;
        public event Action<RoutineDTO>? RoutineEdited;
        public event Action<RoutineDTO>? RoutineDeleted;

        public RoutineService(MainContext context)
        {
            _context = context;
        }

        protected IIncludableQueryable<RoutineDTO, List<SetDTO>> EagerLoad() 
            => _context.Routines.Include(r => r.Workouts)
                                .ThenInclude(w => w.LiftWorkouts)
                                .ThenInclude(lw => lw.Lift)
                                .Include(r => r.Workouts)
                                .ThenInclude(w => w.LiftWorkouts)
                                .ThenInclude(lw => lw.Sets);

        public async Task<IEnumerable<RoutineDTO>> GetAllAsync()
            => await EagerLoad().ToListAsync();

        public async Task<RoutineDTO> GetAsync(int Id)
            => await EagerLoad().SingleAsync(r => r.Id == Id);

        public async Task AddAsync(RoutineDTO routine)
        {   
            await _context.AddAsync(routine);
            await _context.SaveChangesAsync();
            RoutineCreated?.Invoke(routine);
        }

        public async Task UpdateAsync(RoutineDTO routine)
        {
            _context.Update(routine);
            await _context.SaveChangesAsync();
            RoutineEdited?.Invoke(routine);
        }

        public async Task DeleteAsync(RoutineDTO routine)
        {
            _context.Remove(routine);
            await _context.SaveChangesAsync();
            RoutineDeleted?.Invoke(routine);
        }
    }
}
