using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Extensions;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Services;

public class LicensesService : ILicensesService
{
    public async Task AddLicense(License license)
    {
        await using ApplicationDbContext context = new();

        if (!LicenseHelper.IsLicenseValid(license))
            throw new InvalidDataException($"Попытка добавить неверную лицензию");

        await context.Licenses.AddAsync(license);
    }

    public async Task<License?> GetLicenseByKey(string key)
    {
        await using ApplicationDbContext context = new();
        return await context.Licenses.AsNoTracking()
            .FirstOrDefaultAsync(l => l.Key == key);
    }

    public async Task RemoveLicense(string key)
    {
        await using ApplicationDbContext context = new();
        var license = await context.Licenses.FirstOrDefaultAsync(l => l.Key == key);
        if (license is not null)
            context.Licenses.Remove(license);
    }
}
