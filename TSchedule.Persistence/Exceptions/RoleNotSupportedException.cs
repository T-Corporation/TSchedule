using TSchedule.Persistence.Enums;

namespace TSchedule.Persistence.Exceptions;

public class RoleNotSupportedException(Роль roleName)
    : Exception($"Роль {roleName} не поддерживается");
