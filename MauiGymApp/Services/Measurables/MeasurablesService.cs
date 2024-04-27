using MauiGymApp.Contexts;
using MauiGymApp.Models.DTOs.Goals;
using MauiGymApp.Models.DTOs.Measurables;
using MauiGymApp.ViewModels.MeasurableQuantities;
using MauiGymApp.Views.MeasurableQuantities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MauiGymApp.Services.Measurables
{   
    /// <summary>
    /// Stateless service for retreiving infromation
    /// </summary>
    public class MeasurablesService : IMeasurablesService
    {
        private readonly MainContext _context;

        public MeasurablesService(MainContext context)
        {
            _context = context;
        }

        protected IIncludableQueryable<MeasurableQuantityDTO, GoalDTO?> EagerLoad()
            => _context.MeasurableQuantities.Include(m => m.Measurements).Include(m => m.Goal);

        public async Task<IEnumerable<MeasurableQuantityDTO>> GetAllAsync()
            => await EagerLoad().ToListAsync();

        public async Task<MeasurableQuantityDTO> GetAsync(int Id)
            => await EagerLoad().SingleAsync(m => m.Id == Id);

        public async Task AddAsync(MeasurableQuantityDTO quantity)
        {
            await _context.AddAsync(quantity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<MeasurableQuantityDTO> items)
        {
            await _context.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MeasurableQuantityDTO quantity)
        {
            _context.Update(quantity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(MeasurableQuantityDTO quantity)
        {
            _context.Remove(quantity);
            await _context.SaveChangesAsync();
        }

        public async Task AddMeasurementAsync(MeasurableQuantityDTO quantity, MeasurementDTO measurement)
        {
            quantity.Measurements.Add(measurement);
            await UpdateAsync(quantity);
        }

        public async Task UpdateMeasurementAsync(MeasurableQuantityDTO quantity, MeasurementDTO measurement)
        {
            var m = quantity.Measurements.Single(m => m.Id == measurement.Id);
            quantity.Measurements.Remove(m);
            quantity.Measurements.Add(measurement);

            await UpdateAsync(quantity);
        }

        public async Task DeleteMeasurementAsync(MeasurableQuantityDTO quantity, MeasurementDTO measurement)
        {
            quantity.Measurements.Remove(quantity.Measurements.Single(m => m.Id == measurement.Id));
            await UpdateAsync(quantity);
        }
    }
}
