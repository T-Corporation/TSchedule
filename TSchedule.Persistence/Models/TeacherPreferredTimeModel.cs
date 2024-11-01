using CommunityToolkit.Mvvm.ComponentModel;
using TSchedule.Persistence.Entities;
using WeekDay = TSchedule.Persistence.Entities.WeekDay;

namespace TSchedule.Persistence.Models;

public partial class TeacherPreferredTimeModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private TeacherModel? _teacher;

    [ObservableProperty]
    private WeekDay? _dayOfWeek;

    [ObservableProperty]
    private DateTime? _preferredStart;

    [ObservableProperty]
    private DateTime? _preferredEnd;

    public TeacherPreferredTime ToEntity()
        => new()
        {
            Id = Id,
            DayOfWeekId = DayOfWeek!.Id,
            PreferredEnd = PreferredEnd is not null ? TimeOnly.FromDateTime(PreferredEnd.Value) : null,
            PreferredStart = PreferredStart is not null ? TimeOnly.FromDateTime(PreferredStart.Value) : null
        };
}
