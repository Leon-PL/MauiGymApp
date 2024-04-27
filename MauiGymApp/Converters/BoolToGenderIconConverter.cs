using System.Globalization;

namespace MauiGymApp.Converters
{
    public class BoolToGenderIconConverter : IMarkupExtension, IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null) return "";

            if ((bool)value) return "male.png";
            return "female.png";
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
