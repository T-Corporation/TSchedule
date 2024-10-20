using TSchedule.Persistence.Enums;

namespace TSchedule.Persistence.Entities;

public class Administrator : ApplicationUser
{
    public override UserRole Role => UserRole.Administrator;
}
