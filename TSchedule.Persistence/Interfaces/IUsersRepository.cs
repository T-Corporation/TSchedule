using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface IUsersRepository : IRepository
{
    Task<IEnumerable<ApplicationUser>> FindAll(Роль role);
    Task<ApplicationUser> FindById(Guid id, Роль role);
    Task<ApplicationUser> FindByEmail(string email, Роль role);
    Task<ApplicationUser> FindByUserName(string userName, Роль role);
    Task<ApplicationUser> FindByPhoneNumber(string phoneNumber, Роль role);
    Task<IEnumerable<ApplicationUser>> FindByLikeFullName(string fullName, Роль role);

    Task<bool> Create(ApplicationUser user, bool willThrow = false);
    Task<bool> Update(ApplicationUser user, bool willThrow = false);
    Task<bool> DeleteById(Guid id, Роль role, bool willThrow = false);
}
