using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TSchedule.Persistence.Models;

namespace TSchedule.Persistence.Entities;

[Table("Workload", Schema = "Timetable")]
public class Workload
{
    [Key]
    public int Id { get; set; }

    [Required]
    public short Hours { get; set; } // Количество часов (либо в неделю, либо в семестр)

    [Required]
    public bool IsForSemester { get; set; } // True для семестра (для студентов), False для недели (для преподавателей)

    [ForeignKey(nameof(Teacher))]
    public Guid? TeacherId { get; set; } // Нагрузка на преподавателя
    public Teacher? Teacher { get; set; }

    [StringLength(50)]
    [ForeignKey(nameof(Group))]
    public int? GroupId { get; set; } // Нагрузка на студентов
    public Group? Group { get; set; }

    [StringLength(20)]
    [ForeignKey(nameof(Subject))]
    public int SubjectId { get; set; }
    public Subject? Subject { get; set; }

    public WorkloadModel ToModel()
        => new()
        {
            Id = Id,
            Hours = Hours,
            IsForSemester = IsForSemester,
            Teacher = Teacher?.ToModel(),
            Group = Group?.ToModel(),
            Subject = Subject?.ToModel()
        };
}
