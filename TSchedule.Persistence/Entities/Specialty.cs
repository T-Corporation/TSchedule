using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TSchedule.Persistence.Entities;

[Table("Specialties", Schema = "Academic")]
public class Specialty
{
    [Key]
    [StringLength(50)]
    [Required(AllowEmptyStrings = false)]
    public string Code { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;
}
