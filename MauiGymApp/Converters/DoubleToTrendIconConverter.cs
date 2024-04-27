
using System.Globalization;

namespace MauiGymApp.Converters
{
    public class DoubleToTrendIconConverter : IMarkupExtension, IValueConverter
    {
        public object Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
        {
            if (value is null) return "blank_trend";
            var differential = System.Convert.ToDouble(value);

            if (differential > 0) return "upward_trend.png";
            else if (differential == 0) return "dot.png";
            else return "downward_trend.png";
        }

        public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
