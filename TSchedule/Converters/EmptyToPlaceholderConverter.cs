using System.Globalization;
using System.Windows.Data;

namespace TSchedule.Converters;

public class EmptyToPlaceholderConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => string.IsNullOrEmpty(value?.ToString()) ? "<пусто>" : value;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => value;
}

