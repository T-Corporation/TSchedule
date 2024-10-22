using System.Collections.Concurrent;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Managers;

/// <summary>
/// Менеджер для хранения и регистрации сервисов
/// </summary>
public sealed class ServiceManager : IServiceManager, IDisposable
{
    /// <summary>
    /// Закрыт ли ServiceManager
    /// </summary>
    private bool _isDisposed;

    /// <summary>
    /// "Ленивое" создание менеджера по умолчанию
    /// </summary>
    private static readonly Lazy<ServiceManager> _default = new(() => new ServiceManager());

    /// <summary>
    /// Ссылка на менеджер по умолчанию
    /// </summary>
    public static ServiceManager Default => _default.Value;

    /// <summary>
    /// Сервисы
    /// </summary>
    private readonly ConcurrentDictionary<Type, object> _services = new();

    /// <summary>
    /// Добавление сервиса, не зависимого от репозитория
    /// </summary>
    /// <typeparam name="TService">Тип самодостаточного сервиса</typeparam>
    /// <returns>Используемый ServiceManager</returns>
    /// <exception cref="ObjectDisposedException">Вылетает, если сервер не был зарегистрирован</exception>
    /// <example cref="InvalidOperationException">Вылетает, если сервер уже зарегистрирован</example>
    public IServiceManager AddSingleton<TService>() where TService : class, IService, new()
    {
        CheckIfDisposed();
        if (!_services.TryAdd(typeof(TService), new TService()))
        {
            throw new InvalidOperationException($"Сервис \"{typeof(TService).Name}\" уже зарегистрирован.");
        }
        return this;
    }

    /// <summary>
    /// Добавление сервиса, зависящего от репозитория
    /// </summary>
    /// <typeparam name="TRepository">Тип репозитория</typeparam>
    /// <typeparam name="TService">Тип сервиса</typeparam>
    /// <returns>Используемый ServiceManager</returns>
    /// <exception cref="ObjectDisposedException">Вылетает, если сервер не был зарегистрирован</exception>
    /// <example cref="InvalidOperationException">Вылетает, если сервер уже зарегистрирован</example>
    public IServiceManager AddSingleton<TRepository, TService>()
        where TRepository : class, IRepository, new()
        where TService : class, IService
    {
        CheckIfDisposed();
        var repository = new TRepository();
        var service = (TService)Activator.CreateInstance(typeof(TService), repository)!;
        if (!_services.TryAdd(typeof(TService), service))
            throw new InvalidOperationException($"Сервис \"{typeof(TService).Name}\" уже зарегистрирован.");

        return this;
    }

    /// <summary>
    /// Получает запрошенный сервис
    /// </summary>
    /// <typeparam name="TService">Тип сервиса</typeparam>
    /// <returns>Используемый ServiceManager</returns>
    /// <exception cref="InvalidOperationException">Вылетает, если сервер не был зарегистрирован</exception>
    public TService GetRequiredService<TService>() where TService : class, IService
    {
        if (_services.TryGetValue(typeof(TService), out var service))
            return (TService)service;

        throw new InvalidOperationException($"Сервис \"{typeof(TService).Name}\" не зарегистрирован.");
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
    /// <exception cref="ObjectDisposedException">Выбрасывается, если регистрация сервисов закрыта</exception>
    private void CheckIfDisposed()
    {
        if (_isDisposed)
            throw new ObjectDisposedException(
                nameof(ServiceManager),
                $"Настройка {nameof(ServiceManager)} доступна только во время загрузки приложения");
    }
}
