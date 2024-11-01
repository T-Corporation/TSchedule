using CommunityToolkit.Mvvm.ComponentModel;
using TSchedule.Persistence.Entities;
using WeekDay = TSchedule.Persistence.Entities.WeekDay;

namespace TSchedule.Persistence.Models;

public partial class ScheduleModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private WeekDay? _weekDay; // День недели

    [ObservableProperty]
    private TimeOnly _startTime; // Время начала занятия

    [ObservableProperty]
    private TimeOnly _endTime; // Время окончания занятия

    [ObservableProperty]
    private byte _semester; // Полугодие (1 или 2 семестр)

    [ObservableProperty]
    private byte _lessonNumber; // Номер урока (1 - 6)

    [ObservableProperty]
    private short _year; // Год обучения
    
    [ObservableProperty]
    private bool _isDenominator; // Занятие проходит в знаменателе
    
    [ObservableProperty]
    private TeacherModel? _teacher;
    
    [ObservableProperty]
    private GroupModel? _group;
    
    [ObservableProperty]
    private SubjectModel? _subject;
    
    [ObservableProperty]
    private ClassroomModel? _classroom;

    public Schedule ToEntity()
        => new()
        {
            Id = Id,
            WeekDayId = WeekDay!.Id,
            StartTime = StartTime,
            EndTime = EndTime,
            Semester = Semester,
            LessonNumber = LessonNumber,
            Year = Year,
            IsDenominator = IsDenominator,
            TeacherId = Teacher!.Id,
            GroupId = Group!.Id,
            SubjectId = Subject!.Id,
            ClassroomId = Classroom!.Id,
        };
}
