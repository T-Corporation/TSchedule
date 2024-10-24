using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Exceptions;

public class UserNotFoundException(string propertyName, object propertyValue)
    : EntityNotFoundException<ApplicationUser>(propertyName, propertyValue);
