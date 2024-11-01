using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Enums;

namespace TSchedule.Persistence.Models;

public class AdministratorModel : ApplicationUserModel
{
    public override Role Role => Role.Администратор;

    public Administrator ToEntity()
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
