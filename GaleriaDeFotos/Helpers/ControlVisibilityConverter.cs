using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace GaleriaDeFotos.Helpers;

public class ControlVisibilityConverter : IValueConverter
{
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        // Will only parse to True if it's passed an argument of True to the converter in Xaml
        _ = bool.TryParse((string)parameter, out var inverter);

        bool result;

        switch (value)
        {
            case bool boolValue:
            {
                result = boolValue;
                result = inverter ? !result : result;

                return result ? Visibility.Visible : Visibility.Collapsed;
            }
            case int intValue:
            {
                result = intValue is 0;

                result = inverter ? !result : result;

                return result ? Visibility.Visible : Visibility.Collapsed;
            }
            default:
                return Visibility.Collapsed;
        }
    }

    public object? ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return null;
    }

    #endregion
}