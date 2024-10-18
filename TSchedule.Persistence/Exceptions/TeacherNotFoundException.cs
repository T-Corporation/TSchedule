namespace TSchedule.Persistence.Exceptions;

public class TeacherNotFoundException(string propertyName, object propertyValue)
    : Exception($"Преподаватель со значением свойства {propertyName}={propertyValue} не найден");
