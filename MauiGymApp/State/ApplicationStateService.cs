using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiGymApp.State
{
    public class ApplicationStateService : IApplicationStateService
    {
        public ApplicationStateService()
        {
                
        }

        public DateTime SelectedDate { get; set; } = DateTime.Now;
    }
}
