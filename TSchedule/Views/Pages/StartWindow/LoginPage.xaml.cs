using System.Windows;
using TSchedule.ViewModels;

namespace TSchedule.Views.Pages.StartWindow;

public partial class LoginPage
{
    public LoginPage() => InitializeComponent();

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        => ((LoginPageViewModel)DataContext).Password = PasswordBox.Password;
}
