using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface IAdministratorsService : IService
{
    Task<Administrator> GetAdministratorById(Guid id);
    Task<IEnumerable<Administrator>> GetAllAdministrators();
    Task<Administrator> GetAdministratorByEmail(string email);
    Task<Administrator> GetAdministratorByUserName(string userName);
    Task<Administrator> GetAdministratorByPhoneNumber(string phoneNumber);
    Task<IEnumerable<Administrator>> GetAdministratorsByLikeFullName(string fullName);

    Task<bool> AddAdministrator(Administrator admin);
    Task<bool> UpdateAdministrator(Administrator admin);
    Task<bool> RemoveAdministrator(Guid id);
}
