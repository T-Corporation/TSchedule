using TSchedule.Persistence.Enums;

namespace TSchedule.Persistence.Exceptions;

public class RoleNotSupportedException : Exception
{
    public RoleNotSupportedException(Role role) : base($"Роль {role} не поддерживается") { }
    public RoleNotSupportedException(string roleName) : base($"Роль {roleName} не поддерживается") { }
}
