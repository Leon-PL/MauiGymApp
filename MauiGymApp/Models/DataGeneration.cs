using MauiGymApp.Models.DTOs.Measurables;
using MauiGymApp.Models.DTOs.WeightLifting;

namespace MauiGymApp.Models
{
    public static class DataGeneration
    {
        public static IList<LiftDTO> MockLiftData()
        {
            return
            [
                new(){ Id=1,  Name = "Front Squat", MovementPattern=MovementPattern.KneeFlexion },
                new(){ Id=2,  Name = "High Bar Back Squat", MovementPattern=MovementPattern.KneeFlexion },
                new(){ Id=3,  Name = "Low Bar Back Squat", MovementPattern=MovementPattern.KneeFlexion },
                              
                new(){ Id=4,  Name = "Conventional Deadlift", MovementPattern=MovementPattern.HipHinge },
                new(){ Id=5,  Name = "Romanian Deadlift", MovementPattern=MovementPattern.HipHinge },
                new(){ Id=6,  Name = "Stiff Leg Deadlift", MovementPattern=MovementPattern.HipHinge },
                new(){ Id=7,  Name = "Trap Bar Deadlift", MovementPattern=MovementPattern.HipHinge },
                new(){ Id=8,  Name = "Jefferson Deadlift", MovementPattern=MovementPattern.HipHinge },
                new(){ Id=9,  Name = "Snatch Grip Deadlift", MovementPattern=MovementPattern.HipHinge },

                new(){ Id=10, Name = "Overhead Press", MovementPattern = MovementPattern.VerticalPress },
                new(){ Id=11, Name = "BTN Press", MovementPattern = MovementPattern.VerticalPress },
                new(){ Id=12, Name = "Seated Overhead Press", MovementPattern = MovementPattern.VerticalPress },
                new(){ Id=13, Name = "Dumbbell Overhead Press", MovementPattern = MovementPattern.VerticalPress },
            ];
        }

        public static IEnumerable<LiftWorkoutDTO> MockLiftWorkoutData()
        {
            throw new NotImplementedException();
        }

        public static IList<SetDTO> MockSetData()
        {
            return
            [
                new SetDTO(){ Id=1, WeightKg=80, Reps=9 },
                new SetDTO(){ Id=2, WeightKg=70, Reps=14 },
                new SetDTO(){ Id=3, WeightKg=88, Reps=6 },
                new SetDTO(){ Id=4, WeightKg=80, Reps=10 },
                new SetDTO(){ Id=5, WeightKg=80, Reps=10 },
                new SetDTO(){ Id=6, WeightKg=80, Reps=10 },
                new SetDTO(){ Id=7, WeightKg=80, Reps=10 },
                new SetDTO(){ Id=8, WeightKg=80, Reps=10 },
                new SetDTO(){ Id=9, WeightKg=80, Reps=10 },
                new SetDTO(){ Id=10, WeightKg=80, Reps=10 },
                new SetDTO(){ Id=11, WeightKg=80, Reps=10 },
            ];
        }

        public static IList<MeasurableQuantityDTO> MockMeasurableQuantityData()
        {   
            return
            [
                new() { Id=1, Name="Weight", QuantityType=QuantityType.Mass, Notes=""},
                new() { Id=2, Name="Body Fat %", QuantityType=QuantityType.Percentage, Notes=""},
                new() { Id=3, Name="Arm Circumference", QuantityType=QuantityType.Length, Notes="Largest Part of the arm"}
            ];
        }
    }
}
