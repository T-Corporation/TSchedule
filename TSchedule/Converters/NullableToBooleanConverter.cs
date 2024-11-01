using System.Globalization;
using System.Windows.Data;

namespace TSchedule.Converters;

public class NullableToBooleanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        => value is not null;

    public object? ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        => value is bool b && b ? b : null;
}
