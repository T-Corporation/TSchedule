using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSchedule.Persistence.Entities;

[Table("Workload", Schema = "Timetable")]
public class Workload
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int Hours { get; set; } // Количество часов (либо в неделю, либо в семестр)

    [Required]
    public bool IsForSemester { get; set; } // True для семестра (для студентов), False для недели (для преподавателей)

    [ForeignKey(nameof(Teacher))]
    public Guid? TeacherId { get; set; } // Нагрузка на преподавателя
    public Teacher? Teacher { get; set; }

    [ForeignKey(nameof(Group))]
    public int? GroupId { get; set; } // Нагрузка на студентов
    public StudentGroup? Group { get; set; }

    [ForeignKey(nameof(Subject))]
    public int SubjectId { get; set; }
    public Subject? Subject { get; set; }
}
