using System.Globalization;
using System.Windows.Data;

namespace TaxHelper.Converters
{
    internal class DataSourceTypeToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            return value.ToString() == parameter.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolean && boolean && parameter != null)
            {
                return Enum.Parse(targetType, parameter.ToString());
            }

            return Binding.DoNothing;
        }
    }
}
