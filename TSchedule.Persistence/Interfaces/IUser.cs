namespace TSchedule.Persistence.Interfaces;

public interface IUser
{
    Guid Id { get; set; }

    string FullName { get; set; }

    string UserName { get; set; }

    string PasswordHash { get; set; }

    string Email { get; set; }

    string PhoneNumber { get; set; }
}
