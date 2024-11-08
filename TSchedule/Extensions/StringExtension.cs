namespace TSchedule.Extensions;

public static class StringExtension
{
    public static string ToInitials(this string fullName)
    {
        var parts = fullName.Split(' ');

        return parts.Length switch
        {
            1 => $"{fullName[0]}",
            2 or 3 => $"{parts[0][0]}{parts[1][0]}",
            _ => throw new NotSupportedException("Инициалы могут быть только вида \"И\", \"ФИ\", \"ФИО\""),
        };
    }

    public static string ToShortName(this string fullName)
    {
        var parts = fullName.Split(' ');
        var surname = parts[0];
        var name = parts[1][0];
        var fatherName = parts[2][0];
        return $"{surname} {name}.{fatherName}.";
    }
}

