using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface IUsersRepository : IRepository
{
    Task<IEnumerable<ApplicationUser>> FindAll(Role role);
    Task<ApplicationUser> FindById(Guid id, Role role);
    Task<ApplicationUser> FindByEmail(string email, Role role);
    Task<ApplicationUser> FindByUserName(string userName, Role role);
    Task<ApplicationUser> FindByPhoneNumber(string phoneNumber, Role role);
    Task<IEnumerable<ApplicationUser>> FindByLikeFullName(string fullName, Role role);

    Task<bool> Create(ApplicationUser user, bool willThrow = false);
    Task<bool> Update(ApplicationUser user, bool willThrow = false);
    Task<bool> DeleteById(Guid id, Role role, bool willThrow = false);
}
