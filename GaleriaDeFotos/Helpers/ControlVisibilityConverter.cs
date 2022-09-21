using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace GaleriaDeFotos.Helpers;
public class ControlVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        // Will only parse to True if it's passed an argument of True to the converter in Xaml
        _ = bool.TryParse((string)parameter, out bool inverter);

        return value switch
        {
            bool b => b ? inverter ? Visibility.Collapsed : Visibility.Visible : Visibility.Collapsed,
            int i => i > 0 ? inverter ? Visibility.Collapsed : Visibility.Visible : Visibility.Collapsed,
            string s => s is not null && s.Length > 0 ? inverter ? Visibility.Collapsed : Visibility.Visible : Visibility.Collapsed,
            null => inverter ? Visibility.Visible : Visibility.Collapsed,
            _ => Visibility.Collapsed,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return null;
    }
}