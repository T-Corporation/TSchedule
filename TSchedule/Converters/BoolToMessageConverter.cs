using System.Globalization;
using System.Windows.Data;

namespace TSchedule.Converters;

public class BoolToMessageConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is not string s)
            throw new NotSupportedException("Не строковый параметр не поддерживается.");
        
        if (value is not bool b)
            throw new NotSupportedException("Значение должно быть булево.");

        var parts = s.Trim().Split(';');

        if (parts.Length != 2)
            throw new NotSupportedException("В параметре должна быть строка формата \"ответ если true;ответ если false\".");

        var trueMessage = parts[0].Trim();
        var falseMessage = parts[1].Trim();
        return b ? trueMessage : falseMessage;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
