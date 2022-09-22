using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace GaleriaDeFotos.Helpers;
public class ControlVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        // Will only parse to True if it's passed an argument of True to the converter in Xaml
        _ = bool.TryParse((string)parameter, out bool inverter);

        bool result;

        if (value is int @i && @i == 0) result = true;
        else result = false;

        if (inverter) result = !result;

        if (result == true) return Visibility.Visible;
        else return Visibility.Collapsed;

    }

    public object? ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return null;
    }
}