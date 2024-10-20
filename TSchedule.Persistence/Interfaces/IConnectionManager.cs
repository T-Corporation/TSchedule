namespace TSchedule.Persistence.Interfaces;

public interface IConnectionManager : IManager
{
    string? GetConnectionString();
}
