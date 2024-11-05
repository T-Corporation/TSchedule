using TSchedule.Persistence.Models;

namespace TSchedule.Persistence.Extensions;

public static class Lessons
{
    private static readonly LessonModel _first = new()
    {
        Id = 1,
        StartTime = TimeOnly.Parse("8:30"),
        EndTime = TimeOnly.Parse("10:05")
    };

    public static LessonModel First => _first;

    private static readonly LessonModel _second = new()
    {
        Id = 2,
        StartTime = TimeOnly.Parse("10:15"),
        EndTime = TimeOnly.Parse("11:50")
    };

    public static LessonModel Second => _second;

    private static readonly LessonModel _third = new()
    {
        Id = 3,
        StartTime = TimeOnly.Parse("12:30"),
        EndTime = TimeOnly.Parse("14:05")
    };

    public static LessonModel Third => _third;

    private static readonly LessonModel _fourth = new()
    {
        Id = 4,
        StartTime = TimeOnly.Parse("14:15"),
        EndTime = TimeOnly.Parse("15:50")
    };

    public static LessonModel Fourth => _fourth;

    private static readonly LessonModel _fifth = new()
    {
        Id = 5,
        StartTime = TimeOnly.Parse("16:00"),
        EndTime = TimeOnly.Parse("17:35")
    };

    public static LessonModel Fifth => _fifth;

    private static readonly LessonModel _sixth = new()
    {
        Id = 6,
        StartTime = TimeOnly.Parse("17:45"),
        EndTime = TimeOnly.Parse("19:20")
    };

    public static LessonModel Sixth => _sixth;
}
