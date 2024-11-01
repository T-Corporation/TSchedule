using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TSchedule.Persistence.Models;

namespace TSchedule.Persistence.Entities;

[Table("Specialties", Schema = "Academic")]
public class Specialty
{
    [Key] public int Id { get; set; }

    [StringLength(50)]
    [Required(AllowEmptyStrings = false)]
    public string Code { get; set; } = string.Empty;

    [StringLength(255)]
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = string.Empty;

    public override string ToString() => $"{Code} {Name}";

    public SpecialtyModel ToModel()
        => new()
        {
            Id = Id,
            Code = Code,
            Name = Name
        };
}
