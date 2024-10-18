using TSchedule.Persistence.Entities;

namespace TSchedule.Persistence.Interfaces;

public interface ITeachersRepository
{
    Task<IEnumerable<Teacher>> FindAll();

    Task<Teacher> FindById(Guid id);

    Task<Teacher> FindByEmail(string email);

    Task<Teacher> FindByUserName(string userName);

    Task<Teacher> FindByPhoneNumber(string phoneNumber);

    Task<IEnumerable<Teacher>> FindAllByFio(string fio);

    Task<bool> Create(Teacher teacher, bool willThrow);

    Task<bool> Update(Teacher teacher, bool willThrow);

    Task<bool> DeleteById(Guid id, bool willThrow);
}
