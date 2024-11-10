using System.Globalization;
using System.Windows.Data;

namespace TSchedule.Converters;

public class YearBeautifierConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is short year ? $"{year}-{year + 1}" : value;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => value is string s && short.TryParse(s.Split('-')[0], out var year) ? year : value;
}
