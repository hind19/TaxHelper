﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TaxHelper.Converters
{
    public class DataSourceTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;
            return value.ToString() == parameter.ToString() ? Visibility.Visible : Visibility.Collapsed ;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool boolean && boolean && parameter != null)
            {
                return Enum.Parse(targetType, parameter.ToString());
            }
            return Binding.DoNothing;
        }
    }
}
