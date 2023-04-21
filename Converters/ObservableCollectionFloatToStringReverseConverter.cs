using System.Globalization;

namespace MauiCoreLibrary.Converters;

public class ObservableCollectionFloatToStringReverseConverter : IValueConverter
{
    ObservableCollection<float> _collection = new();

    /// <summary>
    /// Converts a collection of floats to a string in reverse order using new line "\n" as separator (the last item is at the top of the string). 
    /// <paramref name="parameter"/> represents how many of the <paramref name="value"/> items will be converted to a string. 
    /// If <paramref name="parameter"/> equals zero all items will be converted.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        _collection = (ObservableCollection<float>)value;

        int numberOfItems;
        string collectionReversed = String.Empty;

        if (parameter == null)
            numberOfItems = _collection.Count;
        else
            numberOfItems = int.Parse((string)parameter);

        if (_collection.Count != 0)
        {
            for (int item = _collection.Count - 1; item >= _collection.Count - numberOfItems; item--)
            {
                if (item >= 0)
                {
                    collectionReversed += _collection[item].ToString("g3");
                    if (item > _collection.Count - numberOfItems && item > 0)
                        collectionReversed += "\n";
                }
                else
                    return collectionReversed;
            }
        }

        return collectionReversed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return _collection; // one way converting ends in stopping displaying items of collection - why?!
    }
}
