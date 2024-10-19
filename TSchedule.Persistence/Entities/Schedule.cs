using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TSchedule.Persistence.Entities;

[Table("Schedule", Schema = "Timetable")]
public class Schedule
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string DayOfWeek { get; set; } = string.Empty; // День недели

    [Required]
    public TimeSpan StartTime { get; set; } // Время начала занятия

    [Required]
    public TimeSpan EndTime { get; set; } // Время окончания занятия

    [Required]
    [Range(1, 2)]
    public int Semester { get; set; } // Полугодие (1 или 2 семестр)

    [Required]
    public int Year { get; set; } // Год обучения

    public bool IsDenominator { get; set; } // Занятие проходит в знаменателе

    // Ссылка на преподавателя
    [ForeignKey(nameof(Teacher))]
    public Guid TeacherId { get; set; }

    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Teacher? Teacher { get; set; }

    // Ссылка на группу
    [ForeignKey(nameof(Group))]
    public int GroupId { get; set; }
    public StudentGroup? Group { get; set; }

    // Ссылка на предмет
    [ForeignKey(nameof(Subject))]
    public int SubjectId { get; set; }

    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Subject? Subject { get; set; }

    // Ссылка на аудиторию
    [ForeignKey(nameof(Classroom))]
    public int ClassroomId { get; set; }

    [DeleteBehavior(DeleteBehavior.NoAction)]
    public Classroom? Classroom { get; set; }
}
