using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Models;

public partial class DailyScheduleModel : ObservableObject
{
    [ObservableProperty]
    public WeekDay _day;

    [ObservableProperty]
    public ObservableCollection<ScheduleModel> _lessons = [];

    public string GetScheduleDetails()
        => string.Join(", ", Lessons.Select(l => $"{l.Subject?.Name} ({l.Teacher?.FullName})"));
}
