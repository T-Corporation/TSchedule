using System.Diagnostics;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Services;

public class TeachersService(ITeachersRepository repository) : ITeachersService
{
    public async Task<IUser?> Authenticate(string username, string password)
    {
        try
        {
            var teacher = await repository.FindByUserName(username);
            if (PasswordService.Default.Verify(password, teacher.PasswordHash))
                return teacher;

            throw new TeacherNotFoundException(
                "UserName & Password", $"UserName={username}; Password={password}");
        }
        catch (TeacherNotFoundException tnfe)
        {
            Debug.WriteLine(tnfe.Message);
            return null;
        }
    }

    public async Task<bool> Register(Teacher teacher) => await repository.Create(teacher);
}
