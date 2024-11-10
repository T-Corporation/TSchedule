using System.Globalization;
using System.Windows.Data;
using TSchedule.Extensions;

namespace TSchedule.Converters;

public class StringToInitialsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is string s && !string.IsNullOrEmpty(s) ? s.ToInitials() : value;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
