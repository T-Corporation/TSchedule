using CommunityToolkit.Mvvm.ComponentModel;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Models;

public partial class ScheduleModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private WeekDay? _weekDay; // День недели

    [ObservableProperty]
    private byte _semester; // Полугодие (1 или 2 семестр)

    [ObservableProperty]
    private LessonModel? _lesson; // Номер урока (1 - 6)

    [ObservableProperty]
    private short _year; // Год обучения
    
    [ObservableProperty]
    private bool _isDenominator; // Занятие проходит в знаменателе
    
    [ObservableProperty]
    private TeacherModel? _teacher;
    
    [ObservableProperty]
    private GroupModel? _group;

    public Schedule ToEntity()
        => new()
        {
            Id = Id,
            WeekDayId = WeekDay!.Id,
            Semester = Semester,
            LessonId = Lesson!.Id,
            Year = Year,
            IsDenominator = IsDenominator,
            TeacherId = Teacher!.Id,
            GroupId = Group!.Id
        };

    public override string ToString()
        => $"""
            День недели={WeekDay};
            Семестр={Semester};
            Урок={Lesson};
            Год={Year};
            Знаменатель={IsDenominator};
            Преподаватель={Teacher};
            Группа={Group}.
            """;
}
