using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TSchedule.Persistence.Entities;

[Table("StudentGroups", Schema = "Academic")]
public class StudentGroup
{
    [Key]
    [StringLength(50)]
    [Required(AllowEmptyStrings = false)]
    public string Code { get; set; } = string.Empty;

    [Range(1, 5)] // Например, курс может быть от 1 до 5
    public byte Course { get; set; }

    [ForeignKey(nameof(Specialty))]
    public string SpecialtyCode { get; set; } = null!;
    public Specialty? Specialty { get; set; }
}
