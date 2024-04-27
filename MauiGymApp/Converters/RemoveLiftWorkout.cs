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
    public class RemoveLiftWorkoutConverter : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var o1 = values[0];
            var o2 = values[1];

            if (o1 == null || o2 == null) return null;


            return new List<object>() { (WorkoutViewModel)o1, (LiftWorkoutViewModel)o2 };

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
