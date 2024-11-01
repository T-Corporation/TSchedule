using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TSchedule.Persistence.Models;

namespace TSchedule.Persistence.Entities;

[Table("Classrooms", Schema = "School")]
public class Classroom
{
    [Key] public int Id { get; set; }

    [StringLength(50)]
    [Required(AllowEmptyStrings = false)]
    public string Number { get; set; } = string.Empty;

    [StringLength(100)]
    public string Type { get; set; } = string.Empty;

    public ClassroomModel ToModel()
        => new()
        {
            Id = Id,
            Number = Number,
            Type = Type
        };
}
