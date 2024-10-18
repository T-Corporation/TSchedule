using System.Diagnostics;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Repositories;

namespace TSchedule.Persistence.Services;

public class TeachersService(TeachersRepository repository) : ITeachersService
{
    public async Task<bool> Authenticate(string username, string password)
    {
        try
        {
            var teacher = await repository.FindByUserName(username);
            return PasswordService.Default.Verify(password, teacher.PasswordHash);
        }
        catch (TeacherNotFoundException tnfe)
        {
            Debug.WriteLine(tnfe.Message);
            return false;
        }
    }

    public async Task<bool> Register(Teacher teacher)
        => await repository.Create(teacher);
}
