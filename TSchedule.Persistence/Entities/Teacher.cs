using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TSchedule.Persistence.Enums;

namespace TSchedule.Persistence.Entities;

[Table("Teachers", Schema = "School")]
public class Teacher : ApplicationUser
{
    [DataType(DataType.Date)]
    public DateOnly? BirthDate { get; set; }

    [StringLength(50)]
    public string Classroom { get; set; } = string.Empty;

    [DataType(DataType.Time)]
    public TimeOnly? PreferredTimeStart { get; set; }

    [DataType(DataType.Time)]
    public TimeOnly? PreferredTimeEnd { get; set; }

    [NotMapped]
    public override Роль Role => Роль.Преподаватель;
}
