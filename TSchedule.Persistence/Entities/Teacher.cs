using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Entities;

[Table("Teachers", Schema = "School")]
public class Teacher : IUser
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [StringLength(255)]
    [Required(AllowEmptyStrings = false)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(20)]
    [Required(AllowEmptyStrings = false)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [StringLength(60)]
    public string PasswordHash { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(255)]
    [Required(AllowEmptyStrings = false)]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [Required(AllowEmptyStrings = false)]
    [StringLength(15, MinimumLength = 15)]
    public string PhoneNumber { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    [StringLength(50)]
    public string Classroom { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    public DateTimeOffset? PreferredTime { get; set; }
}