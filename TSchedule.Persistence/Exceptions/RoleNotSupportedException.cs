using TSchedule.Persistence.Enums;

namespace TSchedule.Persistence.Exceptions;

public class RoleNotSupportedException(UserRole roleName)
    : Exception($"Роль {roleName} не поддерживается");
