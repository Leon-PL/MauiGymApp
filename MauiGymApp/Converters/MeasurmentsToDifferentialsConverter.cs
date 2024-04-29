using MauiGymApp.ViewModels.MeasurableQuantities;
using MauiGymApp.ViewModels.Utilities;
using System.Globalization;

namespace MauiGymApp.Converters
{
    public class MeasurmentsToDifferentialsConverter : IMarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is null) return new List<MeasurementDifferentialViewModel>();

            var measurements = (IEnumerable<MeasurementViewModel>)values[0];
            var unit = (Enum)values[1];

            if (measurements is null || unit is null) return new List<MeasurementDifferentialViewModel>();

            return measurements.ToMeasurementDifferentials(unit);
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}