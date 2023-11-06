namespace MauiCoreLibrary.Converters;

public class DateTimeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            if (parameter is string format)
                return dateTime.ToString(format);
            else
                throw new ArgumentException($"{nameof(parameter)} must be a string.");
        }
        else
            throw new ArgumentException($"{nameof(value)} must be a DateTime.");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
