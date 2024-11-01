using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface IClassroomsService : IService
{
    Task<IEnumerable<Classroom>> GetAllClassrooms();
    Task<Classroom> GetClassroomById(int id);
    Task AddClassroom(Classroom classroom);
    Task UpdateClassroom(Classroom classroom);
    Task RemoveClassroom(int id);
}
