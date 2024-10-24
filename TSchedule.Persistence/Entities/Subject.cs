using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TSchedule.Persistence.Entities;

[Table("Subjects", Schema = "Academic")]
public class Subject
{
    [Key]
    [StringLength(20, MinimumLength = 3)]
    [Required(AllowEmptyStrings = false)]
    public string Code { get; set; } = null!;

    [Required(AllowEmptyStrings = false)]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int WeeklyHours { get; set; }

    [ForeignKey(nameof(Specialty))]
    public string SpecialtyCode { get; set; } = null!;
    public Specialty? Specialty { get; set; }
}
