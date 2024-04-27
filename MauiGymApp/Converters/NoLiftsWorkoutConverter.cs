﻿using MauiGymApp.ViewModels.Workouts;
using System.Globalization;

namespace MauiGymApp.Converters
{
    public class NoLiftWorkoutsConverter : IMarkupExtension, IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null) return true;

            var workout = (WorkoutViewModel)value;

            return workout.LiftWorkouts.Count == 0;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
