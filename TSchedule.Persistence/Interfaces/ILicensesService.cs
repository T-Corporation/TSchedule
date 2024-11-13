using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface ILicensesService : IService
{
    Task<License?> GetLicenseByKey(string key);
    Task AddLicense(License license);
    Task RemoveLicense(string key);
}
