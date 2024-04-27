using MauiGymApp.Models;
using MauiGymApp.Models.DTOs.Goals;
using MauiGymApp.Models.DTOs.Measurables;
using MauiGymApp.Models.DTOs.WeightLifting;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MauiGymApp.Contexts
{
    public class MainContext : DbContext
    {
        public DbSet<LiftDTO> Lifts { get; set; }
        public DbSet<LiftWorkoutDTO> LiftWorkouts { get; set; }
        public DbSet<LiftWorkoutTemplateDTO> LiftWorkoutTemplates { get; set; }
        public DbSet<SetTemplateDTO> SetTemplates { get; set; }
        public DbSet<WorkoutDTO> Workouts { get; set; }
        public DbSet<WorkoutTemplateDTO> WorkoutTemplates { get; set; }
        public DbSet<RoutineDTO> Routines { get; set; }
        public DbSet<MeasurableQuantityDTO> MeasurableQuantities { get; set; }
        public DbSet<MeasurementDTO> Measurements { get; set; }
        public DbSet<GoalDTO> Goals { get; set; }

        public MainContext()
        {
            SQLitePCL.Batteries_V2.Init();

            Directory.CreateDirectory(Path.Combine(FileSystem.Current.AppDataDirectory, "data"));
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite($"Data Source={Path.Combine(FileSystem.Current.AppDataDirectory, "data", "database.db3")}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var liftData = DataGeneration.MockLiftData();
            var setData = DataGeneration.MockSetData();

           modelBuilder.Entity<LiftDTO>().Property(m => m.MovementPattern).HasConversion<string>();

            modelBuilder
                .Entity<MeasurableQuantityDTO>()
                .Property(m => m.QuantityType)
                .HasConversion<string>();

            modelBuilder
                .Entity<MeasurementDTO>()
                .Property(m => m.QuantityType)
                .HasConversion<string>();

            modelBuilder
                .Entity<GoalDTO>()  
                .Property(g => g.QuantityType)
                .HasConversion<string>();

            modelBuilder.Entity<LiftDTO>().HasIndex(l => l.Name).IsUnique(true);
            modelBuilder.Entity<LiftDTO>().HasData(liftData);
            modelBuilder.Entity<RoutineDTO>().HasData(new RoutineDTO()
            {
                Id = 1,
                Name = "Production",
                Notes = "Used to store workouts that do not belong to a specific routine"
            });

            //modelBuilder.Entity<WorkoutTemplateDTO>().HasData(new WorkoutTemplateDTO()
            //{
            //    Id = 1,
            //    Name = "Upper Body",
           
            //    RoutineId = 1,
            //    Notes = "",
            //    DateCreated = DateTime.Now
            //});

            modelBuilder.Entity<SetDTO>().HasData(setData);
            modelBuilder.Entity<MeasurableQuantityDTO>().HasData(DataGeneration.MockMeasurableQuantityData());
        }

        public async Task DeleteAllData()
        {
            Lifts.RemoveRange(Lifts);
            LiftWorkouts.RemoveRange(LiftWorkouts);
            Workouts.RemoveRange(Workouts);
            Routines.RemoveRange(Routines);
            MeasurableQuantities.RemoveRange(MeasurableQuantities);
            Measurements.RemoveRange(Measurements);
            await SaveChangesAsync();
        }
    }
}
