using System.ComponentModel.DataAnnotations.Schema;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Models;

namespace TSchedule.Persistence.Entities;

[Table("Administrators", Schema = "School")]
public class Administrator : ApplicationUser
{
    [NotMapped]
    public override Role Role => Role.Администратор;

    public AdministratorModel ToModel()
        => new()
        {
            Id = Id,
            UserName = UserName,
            PhoneNumber = PhoneNumber,
            IsDeleted = IsDeleted,
            Email = Email,
            FullName = FullName,
            PasswordHash = PasswordHash
        };
}
