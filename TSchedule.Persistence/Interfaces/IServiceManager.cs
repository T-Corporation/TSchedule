namespace TSchedule.Persistence.Interfaces;

public interface IServiceManager : IManager
{
    IServiceManager AddSingleton<TService>() where TService : class, new();
    IServiceManager AddSingleton<TRepository, TService>() 
        where TRepository : class, new() 
        where TService : class;

    TService GetRequiredService<TService>() where TService : class;
}
