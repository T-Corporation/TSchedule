namespace TSchedule.Persistence.Exceptions;

public class UserNotFoundException(string propertyName, object propertyValue)
    : Exception($"Пользователь со значением свойства {propertyName}={propertyValue} не найден");
