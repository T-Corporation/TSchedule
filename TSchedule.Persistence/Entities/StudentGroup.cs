using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TSchedule.Persistence.Entities;

[Table("StudentGroups", Schema = "Academic")]
public class StudentGroup
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string GroupCode { get; set; } = string.Empty;

    [Range(1, 5)] // Например, курс может быть от 1 до 5
    public int Course { get; set; }

    [ForeignKey(nameof(Specialty))]
    public int SpecialtyId { get; set; }
    public Specialty? Specialty { get; set; }
}
