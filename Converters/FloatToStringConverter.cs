using System.Globalization;

namespace MauiCoreLibrary.Converters;

public class FloatToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((float)value).ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Should not be used!");
    }
}
