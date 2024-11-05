using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Views;

namespace TSchedule.ViewModels.Pages.StartWindow;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string _userName = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private Role? _role;

    public Role[] Roles =>
    [
        Persistence.Enums.Role.Преподаватель,
        Persistence.Enums.Role.Администратор
    ];

    [RelayCommand]
    private void GoBack()
        => WindowManager.Default.GetViewModel<Views.StartWindow>()!
            .As<StartWindowViewModel>()!
            .GoBack();

    [RelayCommand]
    private async Task Login()
    {
        if (Role is null)
        {
            ErrorMessage = "Выберите роль";
            return;
        }

        if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Заполните все поля";
            return;
        }

        var usersService = ServiceManager.Default.GetRequiredService<IUsersService>();
        await usersService.Authenticate(UserName, Password, Role.Value);

        if (!usersService.IsAuthenticated())
        {
            ErrorMessage = "Неверный логин или пароль";
            return;
        }

        ErrorMessage = string.Empty;

        PreferencesManager.Default.SetRole(Role.Value.ToString());
        PreferencesManager.Default.SetUserGuid(usersService.GetUserGuid());
        PreferencesManager.Default.SetLoggedIn(true);
        PreferencesManager.Default.Save();

        WindowManager.Default.CreateWindow<Views.MainWindow>();
        WindowManager.Default.CloseWindow<Views.StartWindow>();
    }
}
