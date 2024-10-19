using System.ComponentModel.DataAnnotations;

namespace TSchedule.Persistence.Entities;

public class Group
{
    [Key] public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(20, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [StringLength(20, MinimumLength = 3)]
    public string Specialization { get; set; } = string.Empty;

    // ...
}
