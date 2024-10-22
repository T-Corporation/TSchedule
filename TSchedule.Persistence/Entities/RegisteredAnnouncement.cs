using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSchedule.Persistence.Entities;

[Table("RegisteredAnnouncements", Schema = "School")]
public class RegisteredAnnouncement
{
    [Key] public int Id { get; set; }

    [Required] public DateTime CreatedAt { get; set; } = DateTime.Now; // Дата регистрации

    [Required]
    [ForeignKey(nameof(Announcement))]
    public int AnnouncementId { get; set; } // Ссылка на уведомление

    public Announcement? Announcement { get; set; }

    [Required]
    [ForeignKey(nameof(Administrator))]
    public Guid AdministratorId { get; set; } // Администратор, который зарегистрировал уведомление

    public Administrator? Administrator { get; set; }
}
