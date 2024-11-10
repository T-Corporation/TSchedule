using System.Globalization;
using System.Windows.Data;

namespace TSchedule.Converters;

public class ValueToOpacityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is not null ? 1 : parameter is double d ? d : .5;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
