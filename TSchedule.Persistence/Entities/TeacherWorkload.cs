using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSchedule.Persistence.Entities;

public class TeacherWorkload
{
    [Key] public int Id { get; set; }

    /// <summary>
    /// Внешний ключ на поле Id у преподавателя
    /// </summary>
    [ForeignKey(nameof(Teacher))] public int TeacherId { get; set; }

    /// <summary>
    /// Сам преподаватель, на Id которого ссылается внешний ключ
    /// </summary>
    public Teacher Teacher { get; set; } = null!;
}
