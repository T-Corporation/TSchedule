using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TSchedule.Persistence.Entities;

[Table("Subjects", Schema = "Academic")]
public class Subject
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int WeeklyHours { get; set; }

    [ForeignKey(nameof(Specialty))]
    public int SpecialtyId { get; set; }
    public Specialty? Specialty { get; set; }
}
