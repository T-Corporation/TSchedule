using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TSchedule.Views;

namespace TSchedule.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [RelayCommand]
    private void GoToLoginWindow()
    {
        WindowManager.CreateWindow<LoginWindow>(true, WindowManager.GetWindow<MainWindow>());
    }
}
