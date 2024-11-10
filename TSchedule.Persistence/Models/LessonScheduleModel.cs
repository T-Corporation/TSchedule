using CommunityToolkit.Mvvm.ComponentModel;

namespace TSchedule.Persistence.Models;

public partial class LessonScheduleModel : ObservableObject
{
    [ObservableProperty]
    private LessonModel _lesson = null!;

    [ObservableProperty]
    private ScheduleModel? _monday;

    [ObservableProperty]
    private ScheduleModel? _tuesday;

    [ObservableProperty]
    private ScheduleModel? _wednesday;

    [ObservableProperty]
    private ScheduleModel? _thursday;

    [ObservableProperty]
    private ScheduleModel? _friday;

    [ObservableProperty]
    private ScheduleModel? _saturday;

    [ObservableProperty]
    private ScheduleModel? _sunday;

    public override string ToString() => Lesson.ToString();

    public List<ScheduleModel?> GetLessons() =>
    [
        Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
    ];
}
