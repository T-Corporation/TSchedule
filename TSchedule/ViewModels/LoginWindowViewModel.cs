using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
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

    [RelayCommand]
    private void ShowInfo()
        => WindowManager.ShowMessageBox(
            "Логин и пароль предоставляются администрацией вашего учебного заведения.",
            "Информация",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

    [RelayCommand]
    private async Task Login()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrEmpty(UserName)
            || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Заполните все поля";
            return;
        }

        var user = await App.TeachersService.Authenticate(UserName, Password);

        if (user is null)
        {
            ErrorMessage = "Неверный логин или пароль";
            return;
        }

        App.User = user;
        WindowManager.CreateWindow<MainWindow>();
        WindowManager.CloseWindow<LoginWindow>();
    }
}
