namespace TSchedule.Persistence.Exceptions;

public class EntityNotFoundException<T>(string propertyName, object? propertyValue)
    : Exception($"Сущность типа \"{typeof(T).FullName}\" не найдена по данному запросу: \"{propertyName} = {propertyValue}\"");
