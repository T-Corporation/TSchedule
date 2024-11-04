using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TSchedule.Persistence.Models;

namespace TSchedule.Persistence.Entities;

[Index(nameof(StartTime), nameof(EndTime), IsUnique = true)]
public class Lesson
{
    [Key] public int Id { get; set; }

    public TimeOnly StartTime { get; set; } // Время начала занятия

    public TimeOnly EndTime { get; set; } // Время окончания занятия

    public LessonModel ToModel()
        => new()
        {
            Id = Id,
            StartTime = StartTime,
            EndTime = EndTime
        };
}
