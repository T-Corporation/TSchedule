using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSchedule.Persistence.Entities;

[Table("Announcements", Schema = "School")]
public class Announcement
{
    [Key] public int Id { get; set; }

    [Required] public DateTime CreatedAt { get; set; } = DateTime.Now; // Дата создания уведомления

    [Required] public DateTime AbsentFrom { get; set; } // Дата и время начала отсутствия

    [Required] public DateTime AbsentTo { get; set; } // Дата и время окончания отсутствия

    [Required]
    [ForeignKey(nameof(Teacher))]
    public Guid TeacherId { get; set; } // Преподаватель, который отсутствует

    public Teacher? Teacher { get; set; }

    // Замена преподавателя
    [ForeignKey(nameof(SubstituteTeacher))]
    public Guid? SubstituteTeacherId { get; set; } // Замещающий преподаватель (если есть)

    public Teacher? SubstituteTeacher { get; set; }

    [StringLength(500)]
    public string Reason { get; set; } = string.Empty; // Причина отсутствия

    public bool IsVisibleToGuests { get; set; } // Уведомление видно для гостей
}
