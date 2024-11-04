using CommunityToolkit.Mvvm.ComponentModel;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Models;

public partial class DailyScheduleModel : ObservableObject
{
    [ObservableProperty]
    private WeekDay _day = null!;

    [ObservableProperty]
    private ScheduleModel _monday = null!;

    [ObservableProperty]
    private ScheduleModel _tuesday = null!;

    [ObservableProperty]
    private ScheduleModel _wednesday = null!;

    [ObservableProperty]
    private ScheduleModel _thursday = null!;

    [ObservableProperty]
    private ScheduleModel _friday = null!;

    [ObservableProperty]
    private ScheduleModel _sunday = null!;

    [ObservableProperty]
    private ScheduleModel _saturday = null!;
}
