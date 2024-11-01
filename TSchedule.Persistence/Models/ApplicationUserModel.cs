using CommunityToolkit.Mvvm.ComponentModel;
using TSchedule.Persistence.Enums;

namespace TSchedule.Persistence.Models;

public abstract partial class ApplicationUserModel : ObservableObject
{
    [ObservableProperty]
    private Guid _id;

    [ObservableProperty]
    private string _fullName = string.Empty;

    [ObservableProperty]
    private string _userName = string.Empty;

    [ObservableProperty]
    private string _passwordHash = string.Empty;

    [ObservableProperty]
    private string? _email;

    [ObservableProperty]
    private string? _phoneNumber;

    [ObservableProperty]
    private bool _isDeleted;

    public abstract Role Role { get; }

    public override string ToString() => UserName;
}
