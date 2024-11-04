using CommunityToolkit.Mvvm.ComponentModel;
using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Models;

public partial class LessonModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private TimeOnly _startTime;

    [ObservableProperty]
    private TimeOnly _endTime;

    public Lesson ToEntity()
        => new()
        {
            Id = Id,
            StartTime = StartTime,
            EndTime = EndTime
        };

    public override int GetHashCode()
        => Id.GetHashCode();

    public override bool Equals(object? obj)
        => obj is LessonModel lm && lm.GetHashCode() == GetHashCode();
}
