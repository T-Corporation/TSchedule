using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TSchedule.Persistence.Models;

namespace TSchedule.Persistence.Entities;

[Table("Subjects", Schema = "Academic")]
public class Subject
{
    [Key] public int Id { get; set; }

    [StringLength(20, MinimumLength = 3)]
    [Required(AllowEmptyStrings = false)]
    public string Code { get; set; } = null!;

    [StringLength(255)]
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 36)]
    public byte WeeklyHours { get; set; }

    [ForeignKey(nameof(Specialty))]
    public int SpecialtyId { get; set; }
    public Specialty? Specialty { get; set; }

    public ICollection<GroupSubject> GroupSubjects { get; set; } = [];

    public SubjectModel ToModel()
        => new()
        {
            Id = Id,
            Code = Code,
            Name = Name,
            WeeklyHours = WeeklyHours,
            Specialty = Specialty?.ToModel()
        };
}
