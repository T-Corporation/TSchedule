using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces.Managers;

public interface IServiceManager : IManager
{
    IServiceManager AddSingleton<TService, TImplementation>()
        where TService : class, IService
        where TImplementation : class, TService, new();

    IServiceManager AddSingleton<TService, TImplementation>(Func<TImplementation> factory)
        where TService : class, IService
        where TImplementation : class, TService;

    TService GetRequiredService<TService>()
        where TService : class, IService;
}
