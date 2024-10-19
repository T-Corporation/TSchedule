using System.Windows;
using TSchedule.ViewModels;

namespace TSchedule.Views;

public partial class LoginWindow
{
    public LoginWindow() => InitializeComponent();

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        => WindowManager.GetViewModel<LoginWindow>()!
            .As<LoginWindowViewModel>()!
            .Password = PasswordBox.Password;
}
