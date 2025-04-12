using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TaxHelper.Converters
{
    public class CurrencyConvetrer  : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string currency)
            {
                return currency switch
                {
                    "GBP" => 826,
                    "USD" => 840,
                    "EUR" => 978,
                    "UAN" => 980,
                    "PLN" => 985,
                    _ => 0
                };
            }
            return 0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
