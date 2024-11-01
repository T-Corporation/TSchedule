using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TSchedule.Persistence.Models;

namespace TSchedule.Persistence.Entities;

[Table("Announcements", Schema = "School")]
public class Announcement
{
    [Key] public int Id { get; set; }

    [Required] public DateTime CreatedAt { get; set; } = DateTime.Now; // Дата создания уведомления

    [Required] public DateTime UpdatedAt { get; set; } = DateTime.Now; // Дата создания уведомления

    [Required] public DateTime AbsentFrom { get; set; } // Дата и время начала отсутствия

    [Required] public DateTime AbsentTo { get; set; } // Дата и время окончания отсутствия

    public bool IsRegistered { get; set; }

    [Required]
    [ForeignKey(nameof(Teacher))]
    public Guid TeacherId { get; set; } // Преподаватель, который отсутствует

    public Teacher? Teacher { get; set; }

    [StringLength(500)]
    public string Reason { get; set; } = string.Empty; // Причина отсутствия

    public AnnouncementModel ToModel()
        => new()
        {
            Id = Id,
            IsRegistered = IsRegistered,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            AbsentFrom = AbsentFrom,
            AbsentTo = AbsentTo,
            Reason = Reason,
            Teacher = Teacher?.ToModel()
        };
}
