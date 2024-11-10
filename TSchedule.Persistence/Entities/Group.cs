using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TSchedule.Persistence.Models;

namespace TSchedule.Persistence.Entities;

[Table("Groups", Schema = "Academic")]
public class Group
{
    [Key] public int Id { get; set; }

    [StringLength(50)]
    [Required(AllowEmptyStrings = false)]
    public string Code { get; set; } = string.Empty;

    [Range(1, 5)] // Например, курс может быть от 1 до 5
    public byte Course { get; set; }

    [ForeignKey(nameof(Specialty))]
    public int SpecialtyId { get; set; }
    public Specialty? Specialty { get; set; }

    public ICollection<GroupSubject> GroupSubjects { get; set; } = [];

    public override string ToString() => Code;

    // Почему-то ошибка при генерации Subjects: s.Subject is null
    public GroupModel ToModel()
        => new()
        {
            Id = Id,
            Code = Code,
            Course = Course,
            Subjects = [.. GroupSubjects.Select(s => s.Subject.ToModel())],
            Specialty = Specialty?.ToModel(),
        };
}
