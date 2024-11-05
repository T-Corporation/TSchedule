using System.Windows;
using TSchedule.ViewModels.Pages.StartWindow;

namespace TSchedule.Views.Pages.StartWindow;

public partial class LoginPage
{
    public LoginPage() => InitializeComponent();

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        => ((LoginViewModel)DataContext).Password = PasswordBox.Password;
}
