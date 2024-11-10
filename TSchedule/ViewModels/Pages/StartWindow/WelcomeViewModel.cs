using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Helpers;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.Views;
using TSchedule.Views.Pages.StartWindow;

namespace TSchedule.ViewModels.Pages.StartWindow;

public partial class WelcomeViewModel : ObservableObject
{
    [RelayCommand]
    private void ContinueWithoutAuthentication()
    {
        if (OSVersionHelper.IsWindows10OrGreater && PreferencesManager.Default.IsModernUIEnabled())
        {
            WindowManager.Default.CreateWindow<Views.MainWindow>();
            WindowManager.Default.CloseWindow<Views.StartWindow>();
            return;
        }

        WindowManager.Default.CreateWindow<MainWindowWin7>();
        WindowManager.Default.CloseWindow<StartWindowWin7>();
    }

    [RelayCommand]
    private void ContinueWithAuthentication()
    {
        var viewModel = OSVersionHelper.IsWindows10OrGreater && PreferencesManager.Default.IsModernUIEnabled()
            ? WindowManager.Default.GetViewModel<Views.StartWindow>()!
                .As<StartWindowViewModel>()!
            : WindowManager.Default.GetViewModel<StartWindowWin7>()!
                .As<StartWindowViewModel>()!;

        viewModel.NavigateTo(new LoginPage());
    }
}
