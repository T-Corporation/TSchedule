using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Enums;

namespace TSchedule.Persistence.Interfaces;

public interface IUsersRepository : IRepository
{
    Task<IEnumerable<ApplicationUser>> FindAll(UserRole role);

    Task<ApplicationUser> FindById(Guid id, UserRole role);

    Task<ApplicationUser> FindByEmail(string email, UserRole role);

    Task<ApplicationUser> FindByUserName(string userName, UserRole role);

    Task<ApplicationUser> FindByPhoneNumber(string phoneNumber, UserRole role);

    Task<IEnumerable<ApplicationUser>> FindAllByFullName(string fullName, UserRole role);

    Task<bool> Create(ApplicationUser user, bool willThrow = false);

    Task<bool> Update(ApplicationUser user, bool willThrow = false);

    Task<bool> DeleteById(Guid id, UserRole role, bool willThrow = false);
}
