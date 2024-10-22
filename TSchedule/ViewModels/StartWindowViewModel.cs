using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows;
using TSchedule.Managers;
using TSchedule.Views.Pages.StartWindow;

namespace TSchedule.ViewModels;

public partial class StartWindowViewModel : ObservableObject
{
	/// <summary>
	/// Стек навигации
	/// </summary>
    private readonly Stack<Page> _navigationStack = new();
	
	/// <summary>
	/// Длительность анимации в секундах
	/// </summary>
	private const double AnimationDuration = 0.15;

    [ObservableProperty]
    private Page _currentPage;

    public StartWindowViewModel()
    {
        // Устанавливаем начальную страницу
        CurrentPage = new WelcomePage();
    }

    /// <summary>
	/// Метод для навигации на новую страницу
	/// </summary>
    public void NavigateTo(Page page)
    {
        if (CurrentPage is not null)
            // Добавляем текущую страницу в стек навигации
            _navigationStack.Push(CurrentPage);

        // Анимируем переход (слайд влево для новой страницы)
        AnimatePageTransition(page);
    }

    /// <summary>
	/// Метод для возвращения на предыдущую страницу
	/// </summary>
    public void GoBack()
    {
        if (_navigationStack.Count > 0)
		{
            // Получаем предыдущую страницу из стека
			var previousPage = _navigationStack.Pop();
			AnimatePageTransition(previousPage, isBack: true);
        }
        else
            // Можно добавить обработку, если стек пуст (например, показать предупреждение или ничего не делать)
            WindowManager.ShowMessageBox(
                "Нет доступных страниц для возврата.",
                "Предупреждение",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
    }
	
	/// <summary>
	/// Анимация перехода
	/// </summary>
    private void AnimatePageTransition(Page newPage, bool isBack = false)
    {
        // Создаем анимацию для сдвига по оси X
        TranslateTransform translateTransform = new();
        newPage.RenderTransform = translateTransform;

        // Настраиваем начальное и конечное положение для анимации
        var fromValue = isBack ? -SystemParameters.PrimaryScreenWidth : SystemParameters.PrimaryScreenWidth;
        var toValue = 0.0;

        var slideAnimation = new DoubleAnimation
        {
            To = toValue,
            From = fromValue,
            Duration = TimeSpan.FromSeconds(AnimationDuration),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
        };

        // Устанавливаем анимацию
        translateTransform.BeginAnimation(TranslateTransform.XProperty, slideAnimation);

        // Устанавливаем новую страницу после начала анимации
        CurrentPage = newPage;
    }
}
