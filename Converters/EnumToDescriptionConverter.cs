namespace MauiCoreLibrary.Converters;

public class EnumToDescriptionConverter : IValueConverter // TODO: exctract logic to desciption provider and inject in devicemodel
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        if (targetType != typeof(string))
            throw new ArgumentException("Target type must be System.String.", nameof(targetType));

        FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

        if (fieldInfo == null)
            return value.ToString();

        DescriptionAttribute[] descriptionAttributes = 
            (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (descriptionAttributes is not null && descriptionAttributes.Length > 0)
            return descriptionAttributes[0].Description;
        else
            return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("Should not be used!");
    }
}
