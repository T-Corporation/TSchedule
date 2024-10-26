using System.Collections.Concurrent;
using System.Diagnostics;
using TSchedule.Persistence.Interfaces.Bases;
using TSchedule.Persistence.Interfaces.Managers;

namespace TSchedule.Persistence.Managers;

/// <summary>
/// Менеджер для хранения и регистрации сервисов
/// </summary>
public sealed class ServiceManager : IServiceManager, IDisposable
{
    #pragma warning disable CA1821
    ~ServiceManager()
    #pragma warning restore CA1821
    {
        Debug.WriteLine("ServiceManager был уничтожен.");
    }
    
    #region Private Fields

    private bool _isDisposed;
    private static readonly Lazy<ServiceManager> _default = new(() => new ServiceManager());
    private readonly ConcurrentDictionary<Type, IService> _services = new();

    #endregion

    #region Properties

    public static ServiceManager Default => _default.Value;

    #endregion

    #region Methods

    /// <summary>
    /// Общий метод по добавлению сервиса с конкретной реализацией
    /// </summary>
    /// <typeparam name="TService">Тип сервиса</typeparam>
    /// <typeparam name="TImplementation">Тип реализации сервиса</typeparam>
    /// <param name="service">Сервис</param>
    /// <returns>Используемый ServiceManager</returns>
    /// <exception cref="InvalidOperationException"></exception>
    private ServiceManager AddSimpleSingleton<TService, TImplementation>(TImplementation service)
        where TService : class, IService
        where TImplementation : class, TService
    {
        CheckIfDisposed();
        if (!_services.TryAdd(typeof(TService), service))
            throw new InvalidOperationException(
                $"Сервис \"{typeof(TService).Name}\" уже зарегистрирован.");
        return this;
    }

    /// <summary>
    /// Добавление сервиса с конкретной реализацией
    /// </summary>
    /// <typeparam name="TService">Тип сервиса</typeparam>
    /// <typeparam name="TImplementation">Тип реализации сервиса</typeparam>
    /// <returns>Используемый ServiceManager</returns>
    public IServiceManager AddSingleton<TService, TImplementation>()
        where TService : class, IService
        where TImplementation : class, TService, new()
        => AddSimpleSingleton<TService, TImplementation>(new TImplementation());

    /// <summary>
    /// Добавление сервиса с конкретной реализацией и фабрикой
    /// </summary>
    /// <param name="factory">Фабрика по созданию экземпляра сервиса</param>
    /// <typeparam name="TService">Тип сервиса</typeparam>
    /// <typeparam name="TImplementation">Тип реализации сервиса</typeparam>
    /// <returns>Используемый ServiceManager</returns>
    public IServiceManager AddSingleton<TService, TImplementation>(Func<TImplementation> factory)
        where TService : class, IService
        where TImplementation : class, TService
        => AddSimpleSingleton<TService, TImplementation>(factory());

    /// <summary>
    /// Получает запрошенный сервис
    /// </summary>
    /// <typeparam name="TService">Тип сервиса</typeparam>
    /// <returns>Используемый ServiceManager</returns>
    /// <exception cref="InvalidOperationException">Вылетает, если сервер не был зарегистрирован</exception>
    public TService GetRequiredService<TService>()
        where TService : class, IService
    {
        if (!_services.TryGetValue(typeof(TService), out var service))
            throw new InvalidOperationException($"Сервис \"{typeof(TService).Name}\" не зарегистрирован.");
        return (TService)service;
    }

    /// <summary>
    /// Отключение регистрации сервисов
    /// </summary>
    public void Dispose()
    {
        _isDisposed = true;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Проверяет, закрыта ли регистрация сервисов и если да, то выбрасывает <see cref="ObjectDisposedException"/>
    /// </summary>
    private void CheckIfDisposed()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(
                nameof(ServiceManager),
                "Регистрация новых сервисов завершена. Инициализация сервисов доступна только на этапе старта приложения.");
    }

    #endregion
}
