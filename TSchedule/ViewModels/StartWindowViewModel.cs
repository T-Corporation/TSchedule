using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using TSchedule.Managers;
using TSchedule.Views.Pages.StartWindow;

namespace TSchedule.ViewModels;

public partial class StartWindowViewModel : ObservableObject
{
    private readonly Stack<Page> _navigationStack = new(); // Стек навигации

    [ObservableProperty]
    private Page _currentPage;

    public StartWindowViewModel()
    {
        // Устанавливаем начальную страницу
        CurrentPage = new WelcomePage();
    }

    // Метод для навигации на новую страницу
    public void NavigateTo(Page page)
    {
        if (CurrentPage is not null)
            // Добавляем текущую страницу в стек навигации
            _navigationStack.Push(CurrentPage);

        // Устанавливаем новую страницу
        CurrentPage = page;
    }

    // Метод для возвращения на предыдущую страницу
    public void GoBack()
    {
        if (_navigationStack.Count > 0)
            // Получаем предыдущую страницу из стека
            CurrentPage = _navigationStack.Pop();
        
        else
            // Можно добавить обработку, если стек пуст (например, показать предупреждение или ничего не делать)
            WindowManager.ShowMessageBox(
                "Нет доступных страниц для возврата.",
                "Предупреждение",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
    }
}
