using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace TSchedule.ViewModels;

public partial class LoginWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string userName = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [RelayCommand]
    private void ShowInfo()
        => WindowManager.ShowMessageBox(
            "Логин и пароль предоставляются администрацией вашего учебного заведения.",
            "Информация",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

    [RelayCommand]
    private void Login()
    {
        if (string.IsNullOrEmpty(UserName)
            || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Заполните все поля";
        }

        if ()
    }
}
