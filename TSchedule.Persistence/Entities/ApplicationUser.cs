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
    [Required(AllowEmptyStrings = false)]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [Required(AllowEmptyStrings = false)]
    [StringLength(15, MinimumLength = 15)]
    public string PhoneNumber { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    public abstract Роль Role { get; }
}
