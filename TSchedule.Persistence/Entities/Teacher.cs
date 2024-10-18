using System.ComponentModel.DataAnnotations;

namespace TSchedule.Persistence.Entities;

public class Teacher
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    [Required(AllowEmptyStrings = false)]
    [StringLength(20, MinimumLength = 3)]
    public string UserName { get; set; } = string.Empty;

    [StringLength(100)]
    [Required(AllowEmptyStrings = false)]
    public string PasswordHash { get; set; } = string.Empty;

    [StringLength(50)]
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    [Required(AllowEmptyStrings = false)]
    public string Surname { get; set; } = string.Empty;

    [StringLength(50)]
    public string? FatherName { get; set; }

    [EmailAddress]
    [StringLength(200)]
    [Required(AllowEmptyStrings = false)]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [StringLength(15)]
    [Required(AllowEmptyStrings = false)]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(255)]
    public string ProfilePicture { get; set; } = string.Empty;
}
