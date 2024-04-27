using MauiGymApp.Contexts;
using MauiGymApp.Models.DTOs.Goals;
using MauiGymApp.Models.DTOs.WeightLifting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MauiGymApp.Services.WeightLifting
{
    public class LiftService : ILiftService
    {
        private readonly MainContext _context;

        public event Action<LiftDTO>? LiftCreated;
        public event Action<IEnumerable<LiftDTO>>? LiftsCreated;
        public event Action<LiftDTO>? LiftEdited;
        public event Action<LiftDTO>? LiftDeleted;

        public LiftService(MainContext context)
        {
            _context = context;
        }

        protected IIncludableQueryable<LiftDTO, GoalDTO?> EagerLoad()
           => _context.Lifts.Include(l => l.E1RMGoal);

        public async Task<IEnumerable<LiftDTO>> GetAllAsync()
            => await _context.Lifts.Include(l => l.E1RMGoal).ToListAsync();

        public async Task<LiftDTO> GetAsync(int Id)
        {
            var s =  await _context.Lifts.Include(l => l.E1RMGoal).SingleAsync(m => m.Id == Id);
            return s;
        }

        public async Task AddAsync(LiftDTO lift)
        {
            await _context.AddAsync(lift);
            await _context.SaveChangesAsync();
            LiftCreated?.Invoke(lift);
        }
        public async Task AddRangeAsync(IEnumerable<LiftDTO> lifts)
        {
            await _context.AddRangeAsync(lifts);
            await _context.SaveChangesAsync();
            LiftsCreated?.Invoke(lifts);
        }

        public async Task UpdateAsync(LiftDTO lift)
        {
            _context.Update(lift);
            await _context.SaveChangesAsync();
            LiftEdited?.Invoke(lift);
        }

        public async Task DeleteAsync(LiftDTO lift)
        {
            _context.Remove(lift);
            await _context.SaveChangesAsync();
            LiftDeleted?.Invoke(lift);
        }
    }
}
