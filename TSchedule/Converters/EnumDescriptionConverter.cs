using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace TSchedule.Converters;

public class EnumDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (field?.GetCustomAttribute(typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                return attribute.Description;
        }
        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}