using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.Views;
using TSchedule.Views.Pages.StartWindow;

namespace TSchedule.ViewModels;

public partial class WelcomePageViewModel : ObservableObject
{
    [RelayCommand]
    private void ContinueWithoutAuthentication()
    {
        WindowManager.Default.CreateWindow<MainWindow>();
        WindowManager.Default.CloseWindow<StartWindow>();
    }

    [RelayCommand]
    private void ContinueWithAuthentication()
        => WindowManager.Default.GetViewModel<StartWindow>()!.As<StartWindowViewModel>()!.NavigateTo(new LoginPage());
}
