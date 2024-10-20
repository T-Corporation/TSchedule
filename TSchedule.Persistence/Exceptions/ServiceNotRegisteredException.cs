namespace TSchedule.Persistence.Exceptions;

public class ServiceNotRegisteredException(Type serviceType)
    : Exception($"Сервис с типом \"{serviceType.Name}\" не был зарегистрирован");
