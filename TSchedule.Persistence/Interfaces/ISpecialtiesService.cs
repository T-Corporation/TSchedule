using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface ISpecialtiesService : IService
{
    Task<IEnumerable<Specialty>> GetAllSpecialties();
    Task<Specialty> GetSpecialtyById(int id);
    Task AddSpecialty(Specialty specialty);
    Task UpdateSpecialty(Specialty specialty);
    Task RemoveSpecialty(int id);
}
