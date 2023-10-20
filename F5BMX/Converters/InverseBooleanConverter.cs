using System;
using System.Globalization;
using System.Windows.Data;

namespace F5BMX.Converters;

internal class InverseBooleanConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return false;

        if (System.Convert.ToBoolean(value) == true)
            return false;

        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}
