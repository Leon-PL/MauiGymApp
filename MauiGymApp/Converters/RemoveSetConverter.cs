using MauiGymApp.ViewModels.Workouts;
using MauiGymApp.ViewModels.Workouts.Lifts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiGymApp.Converters
{
    public class RemoveSetConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            LiftWorkoutViewModel lw = (LiftWorkoutViewModel)values[0];
            SetViewModel set = (SetViewModel)values[1];

            return new { LiftWorkout = lw, Set = set };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
