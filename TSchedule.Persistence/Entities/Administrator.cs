using System.ComponentModel.DataAnnotations.Schema;
using TSchedule.Persistence.Enums;

namespace TSchedule.Persistence.Entities;

[Table("Administrators", Schema = "School")]
public class Administrator : ApplicationUser
{
    [NotMapped]
    public override Role Role => Role.Администратор;
}
