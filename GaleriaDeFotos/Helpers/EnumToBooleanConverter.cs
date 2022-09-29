using System.Reflection;
using Microsoft.UI.Xaml.Data;

namespace GaleriaDeFotos.Helpers;

/// <summary>
///     Convert an <see cref="Enum" /> to corresponding <see cref="bool" />
/// </summary>
public class EnumToBoolConverter : IValueConverter
{
    /// <summary>
    ///     Enum values, that converts to <c>true</c> (optional)
    /// </summary>
    // ReSharper disable once CollectionNeverUpdated.Global
    public IList<Enum> TrueValues { get; } = new List<Enum>();

    #region IValueConverter Members

    /// <summary>
    ///     Convert an <see cref="Enum" /> to corresponding <see cref="bool" />
    /// </summary>
    /// <param name="value"><see cref="Enum" /> value to convert</param>
    /// <param name="targetType">The type of the binding target property. This is not implemented.</param>
    /// <param name="parameter">
    ///     Additional parameter for converter. Can be used for comparison instead of
    ///     <see cref="TrueValues" />
    /// </param>
    /// <param name="culture">The culture to use in the converter. This is not implemented.</param>
    /// <returns>
    ///     False, if the value is not in <see cref="TrueValues" />. False, if <see cref="TrueValues" /> is empty and
    ///     value not equal to parameter.
    /// </returns>
    /// <exception cref="ArgumentException">If value is not an <see cref="Enum" /></exception>
    public object Convert(object value, Type targetType, object parameter, string culture)
    {
        if (value is not Enum enumValue)
            throw new ArgumentException("The value should be of type Enum", nameof(value));
        if (parameter is string paramString) parameter = Enum.Parse(value.GetType(), paramString);

        return TrueValues.Count == 0
            ? CompareTwoEnums(enumValue, parameter as Enum)
            : TrueValues.Any(item => CompareTwoEnums(enumValue, item));

        static bool CompareTwoEnums(Enum valueToCheck, object? referenceValue)
        {
            if (referenceValue is not Enum referenceEnumValue) return false;

            var valueToCheckType = valueToCheck.GetType();
            if (valueToCheckType != referenceEnumValue.GetType()) return false;

            if (valueToCheckType.GetTypeInfo().GetCustomAttribute<FlagsAttribute>() != null)
                return referenceEnumValue.HasFlag(valueToCheck);

            return Equals(valueToCheck, referenceEnumValue);
        }
    }


    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }

    #endregion
}