using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiGymApp.Models.DTOs;

namespace MauiGymApp.Models.DTOs.WeightLifting
{
    public sealed class RoutineDTO : BaseDTO
    {
        public string Name { get; set; } = "Routine";
        public string Notes { get; set; } = "";
        public List<WorkoutDTO> Workouts { get; set; } = [];
        public List<WorkoutTemplateDTO> WorkoutTemplates { get; set; } = [];
    }
}
