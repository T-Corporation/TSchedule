using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Services;
using TSchedule.Views;

namespace TSchedule.ViewModels;

public partial class LoginPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string _userName = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private Роль _role;

    public Роль[] Roles =>
    [
        Роль.Преподаватель,
        Роль.Администратор
    ];

    public LoginPageViewModel()
    {
        Role = Roles[0];
    }

    [RelayCommand]
    private void ShowInfo() => WindowManager.ShowMessageBox(
        "Логин и пароль предоставляются администрацией вашего учебного заведения.",
        "Информация",
        MessageBoxButton.OK,
        MessageBoxImage.Information);

    [RelayCommand]
    private void GoBack()
        => WindowManager.Default.GetViewModel<StartWindow>()!.As<StartWindowViewModel>()!.GoBack();

    [RelayCommand]
    private async Task Login()
    {
        if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Заполните все поля";
            return;
        }

        var usersService = ServiceManager.Default.GetRequiredService<UsersService>();
        await usersService.Authenticate(UserName, Password, Role);

        if (!usersService.IsAuthenticated())
        {
            ErrorMessage = "Неверный логин или пароль";
            return;
        }

        ErrorMessage = string.Empty;

        WindowManager.Default.CreateWindow<MainWindow>();
        WindowManager.Default.CloseWindow<StartWindow>();
    }
}
