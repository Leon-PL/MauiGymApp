using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiGymApp.State
{   
    /// <summary>
    /// Holds state for common applciation data
    /// </summary>
    public interface IApplicationStateService
    {
        public DateTime SelectedDate { get; set; }
    }
}
