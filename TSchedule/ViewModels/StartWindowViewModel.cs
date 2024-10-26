using CommunityToolkit.Mvvm.ComponentModel;
using TSchedule.Views.Pages.StartWindow;
using iNKORE.UI.WPF.Modern.Controls;

namespace TSchedule.ViewModels;

public class StartWindowViewModel : ObservableObject
{
    /// <summary>
    /// Фрейм навигации
    /// </summary>
    private Frame NavigationFrame { get; }

    public StartWindowViewModel(Frame navigationFrame)
    {
        NavigationFrame = navigationFrame;
        NavigationFrame.Navigate(new WelcomePage());
    }

    /// <summary>
	/// Метод для навигации на новую страницу
	/// </summary>
    public void NavigateTo(Page page) => NavigationFrame.Navigate(page);

    /// <summary>
	/// Метод для возвращения на предыдущую страницу
	/// </summary>
    public void GoBack() => NavigationFrame.GoBack();
}
