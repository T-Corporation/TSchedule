using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces.Managers;

public interface IConnectionManager : IManager
{
    string? GetConnectionString();
}
