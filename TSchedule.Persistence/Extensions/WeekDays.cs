using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Extensions;

public static class WeekDays
{
    private static readonly WeekDay _monday = new()
    {
        Id = 1,
        Name = "Понедельник"
    };

    public static WeekDay Monday => _monday;

    private static readonly WeekDay _tuesday = new()
    {
        Id = 2,
        Name = "Вторник"
    };

    public static WeekDay Tuesday => _tuesday;

    private static readonly WeekDay _wednesday = new()
    {
        Id = 3,
        Name = "Среда"
    };

    public static WeekDay Wednesday => _wednesday;

    private static readonly WeekDay _thursday = new()
    {
        Id = 4,
        Name = "Четверг"
    };

    public static WeekDay Thursday => _thursday;

    private static readonly WeekDay _friday = new()
    {
        Id = 5,
        Name = "Пятница"
    };

    public static WeekDay Friday => _friday;

    private static readonly WeekDay _saturday = new()
    {
        Id = 6,
        Name = "Суббота"
    };

    public static WeekDay Saturday => _saturday;

    private static readonly WeekDay _sunday = new()
    {
        Id = 7,
        Name = "Воскресенье"
    };

    public static WeekDay Sunday => _sunday;
}
