using System.Windows;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.ViewModels;

namespace TSchedule.Views;

public partial class LoginWindow
{
    public LoginWindow() => InitializeComponent();

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        => WindowManager.Default.GetViewModel<LoginWindow>()!
            .As<LoginWindowViewModel>()!
            .Password = PasswordBox.Password;
}
