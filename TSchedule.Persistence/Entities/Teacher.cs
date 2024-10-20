using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TSchedule.Persistence.Enums;

namespace TSchedule.Persistence.Entities;

[Table("Teachers", Schema = "School")]
public class Teacher : ApplicationUser
{
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    [StringLength(50)]
    public string Classroom { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    public DateTimeOffset? PreferredTime { get; set; }

    public override UserRole Role => UserRole.Teacher;
}
