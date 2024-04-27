using CommunityToolkit.Mvvm.ComponentModel;
using MauiGymApp.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiGymApp.ViewModels.Workouts
{
    public partial class CopyPreviousWorkoutViewModel : BaseViewModel
    {
        public CopyPreviousWorkoutViewModel()
        {

        }

        [ObservableProperty]
        ObservableCollection<DateTime> dates;
    }
}
