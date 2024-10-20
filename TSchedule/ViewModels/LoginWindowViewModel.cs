using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using TSchedule.Managers;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Services;
using TSchedule.Views;

namespace TSchedule.ViewModels;

public partial class LoginWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string userName = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private UserRole userRole = UserRole.Guest;

    [RelayCommand]
    private void ShowInfo() => WindowManager.ShowMessageBox(
        "Логин и пароль предоставляются администрацией вашего учебного заведения.",
        "Информация",
        MessageBoxButton.OK,
        MessageBoxImage.Information);

    [RelayCommand]
    private async Task Login()
    {
        if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Заполните все поля";
            return;
        }

        var usersService = ServiceManager.Default.GetRequiredService<UsersService>();
        await usersService.Authenticate(UserName, Password, UserRole);

        if (!usersService.IsAuthenticated())
        {
            ErrorMessage = "Неверный логин или пароль";
            return;
        }

        ErrorMessage = string.Empty;

        WindowManager.Default.CreateWindow<MainWindow>();
        WindowManager.Default.CloseWindow<LoginWindow>();
    }
}
