using System.ComponentModel.DataAnnotations.Schema;
using TSchedule.Persistence.Enums;

namespace TSchedule.Persistence.Entities;

[Table("Administrators", Schema = "School")]
public class Administrator : ApplicationUser
{
    [NotMapped]
    public override Роль Role => Роль.Администратор;
}
