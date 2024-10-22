namespace TSchedule.Persistence.Interfaces;

public interface IServiceManager : IManager
{
    IServiceManager AddSingleton<TService>() where TService : class, IService, new();

    IServiceManager AddSingleton<TRepository, TService>() 
        where TRepository : class, IRepository, new()
        where TService : class, IService;

    TService GetRequiredService<TService>() where TService : class, IService;
}
