using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Interfaces;

public interface IUsersRepository
{
    Task<IEnumerable<IUser>> FindAll();

    Task<IUser> FindById(Guid id);

    Task<IUser> FindByEmail(string email);

    Task<IUser> FindByUserName(string userName);

    Task<IUser> FindByPhoneNumber(string phoneNumber);

    Task<IEnumerable<IUser>> FindAllByFullName(string fullName);

    Task<bool> Create(IUser user, bool willThrow = false);

    Task<bool> Update(IUser user, bool willThrow = false);

    Task<bool> DeleteById(Guid id, bool willThrow = false);
}
