using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Models;

namespace TSchedule.Persistence.Entities;

[Table("Schedule", Schema = "Timetable")]
[Index(nameof(WeekDayId), nameof(LessonId), nameof(TeacherId), IsUnique = true, Name = "IX_Schedule_Teacher_Time")]
[Index(nameof(WeekDayId), nameof(LessonId), nameof(ClassroomId), IsUnique = true, Name = "IX_Schedule_Classroom_Time")]
public class Schedule
{
    [Key] public int Id { get; set; }

    [Required]
    [Range(1, 2)]
    public byte Semester { get; set; }

    [ForeignKey(nameof(Lesson))]
    public int LessonId { get; set; }
    public Lesson? Lesson { get; set; }

    [Required]
    public short Year { get; set; }

    public bool IsDenominator { get; set; }

    [ForeignKey(nameof(Teacher))]
    public Guid TeacherId { get; set; }
    public Teacher? Teacher { get; set; }

    [ForeignKey(nameof(Group))]
    public int GroupId { get; set; }
    public Group? Group { get; set; }

    [ForeignKey(nameof(Subject))]
    public int SubjectId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Subject? Subject { get; set; }

    [ForeignKey(nameof(Classroom))]
    public int ClassroomId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Classroom? Classroom { get; set; }

    [ForeignKey(nameof(WeekDay))]
    public int WeekDayId { get; set; }
    public WeekDay? WeekDay { get; set; }

    public ScheduleModel ToModel()
        => new()
        {
            Id = Id,
            WeekDay = WeekDay,
            Semester = Semester,
            Lesson = Lesson?.ToModel(),
            Year = Year,
            IsDenominator = IsDenominator,
            Teacher = Teacher?.ToModel(),
            Group = Group?.ToModel(),
            Subject = Subject?.ToModel(),
            Classroom = Classroom?.ToModel(),
        };
}
