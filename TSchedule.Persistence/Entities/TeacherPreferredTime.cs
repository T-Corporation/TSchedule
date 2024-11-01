using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TSchedule.Persistence.Models;

namespace TSchedule.Persistence.Entities;

[Table("TeachersPreferredTimes", Schema = "Academic")]
public class TeacherPreferredTime
{
    [Key] public int Id { get; set; }
    
    // Внешний ключ на Teacher
    [ForeignKey(nameof(TeacherId))]
    public Guid TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;
    
    // Внешний ключ на DayOfWeek
    [ForeignKey(nameof(DayOfWeekId))]
    public int DayOfWeekId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public WeekDay DayOfWeek { get; set; } = null!;
    
    public TimeOnly? PreferredStart { get; set; }
    public TimeOnly? PreferredEnd { get; set; }

    public TeacherPreferredTimeModel ToModel()
        => new()
        {
            Id = Id,
            DayOfWeek = DayOfWeek,
            PreferredEnd = PreferredEnd is not null ? new DateTime(DateOnly.MinValue, PreferredEnd.Value) : null,
            PreferredStart = PreferredStart is not null ? new DateTime(DateOnly.MinValue, PreferredStart.Value) : null
        };
}
