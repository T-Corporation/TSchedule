using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.Views.Pages.StartWindow;

namespace TSchedule.ViewModels.Pages.StartWindow;

public partial class WelcomeViewModel : ObservableObject
{
    [RelayCommand]
    private void ContinueWithoutAuthentication()
    {
        WindowManager.Default.CreateWindow<Views.MainWindow>();
        WindowManager.Default.CloseWindow<Views.StartWindow>();
    }

    [RelayCommand]
    private void ContinueWithAuthentication()
        => WindowManager.Default.GetViewModel<Views.StartWindow>()!
            .As<StartWindowViewModel>()!
            .NavigateTo(new LoginPage());
}
