using System.Windows;
using System.Windows.Data;

namespace TaxHelper.Converters
{
    public class DataSourceTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is null || parameter is null)
            {
                return false;
            }

            return value.ToString() == parameter.ToString() ? Visibility.Visible : Visibility.Collapsed ;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool boolean && boolean && parameter is not null)
            {
                return Enum.Parse(targetType, parameter.ToString());
            }
            return Binding.DoNothing;
        }
    }
}
