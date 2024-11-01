using System.ComponentModel.DataAnnotations;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Entities;

public abstract class ApplicationUser : IUser
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
    public string? Email { get; set; }

    [Phone]
    [StringLength(11, MinimumLength = 11)]
    public string? PhoneNumber { get; set; }

    public bool IsDeleted { get; set; }

    public abstract Role Role { get; }

    public override string ToString() => UserName;
}
