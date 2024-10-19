using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TSchedule.Persistence.Entities;

[Table("Specialties", Schema = "Academic")]
public class Specialty
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string Code { get; set; } = string.Empty;
}
