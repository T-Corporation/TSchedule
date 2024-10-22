using CommunityToolkit.Mvvm.ComponentModel;
using iNKORE.UI.WPF.Modern.Controls.Helpers;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Managers;

public class WindowManager : IManager
{
    #region Fields

    /// <summary>
    /// "Ленивое" создание менеджера по умолчанию
    /// </summary>
    private static readonly Lazy<WindowManager> _instance = new(() => new WindowManager());

    /// <summary>
    /// Ссылка на менеджер по умолчанию
    /// </summary>
    public static WindowManager Default => _instance.Value;

    /// <summary>
    /// Коллекция зарегистрированных окон
    /// </summary>
    private readonly Dictionary<Type, Window> _windows = [];

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

    #endregion

    #region Inner methods

    /// <summary>
    /// Создаёт и регистрирует окно
    /// </summary>
    /// <typeparam name="T">Тип окна</typeparam>
    /// <param name="isModern">Если <b>true</b>, то современный дизайн, иначе обычный</param>
    /// <param name="showDialog">Если <b>true</b>, то запуск в диалоговом режиме, иначе – в обычном</param>
    /// <param name="owner">Владелец окна</param>
    /// <returns></returns>
    public T CreateWindow<T>(bool isModern = true, bool showDialog = false, Window? owner = null)
        where T : Window, new()
    {
        var windowType = typeof(T);
        if (_windows.TryGetValue(windowType, out var value))
            return (T)value;

        var window = new T();
        return CreateWindow(window, isModern, showDialog, owner);
    }

    /// <summary>
    /// Получает зарегистрированное окно по типу
    /// </summary>
    /// <typeparam name="T">Тип окна, которое создано через менеджер</typeparam>
    /// <returns>Если окно найдено, то возвращает его, иначе <b>null</b></returns>
    public T? GetWindow<T>()
        where T : Window
    {
        var windowType = typeof(T);
        return _windows.TryGetValue(windowType, out var window)
            ? (T)window
            : null;
    }

    /// <summary>
    /// Получает DataContext у окна
    /// </summary>
    /// <typeparam name="T">Тип окна, которое создано через менеджер</typeparam>
    /// <returns>DataContext окна или <b>null</b>, если он отсутствует</returns>
    public ObservableObject? GetViewModel<T>()
        where T : Window
        => GetWindow<T>()?.DataContext as ObservableObject;

    

    /// <summary>
    /// Показывает окно, если оно скрыто
    /// </summary>
    /// <typeparam name="T">Тип окна, которое создано через менеджер</typeparam>
    /// <param name="showDialog">Если <b>true</b>, то открывает в режиме диалога</param>
    public void ShowWindow<T>(bool showDialog = false)
        where T : Window, new()
    {
        var window = CreateWindow<T>();
        ShowWindow(window, showDialog);
    }

    /// <summary>
    /// Скрывает окно
    /// </summary>
    /// <typeparam name="T">Тип окна, которое создано через менеджер</typeparam>
    public void MinimizeWindow<T>()
        where T : Window
    {
        var window = GetWindow<T>();
        if (window is null || window.WindowState is WindowState.Minimized)
            return;

        window.WindowState = WindowState.Minimized;
    }

    /// <summary>
    /// Делает окно на весь экран
    /// </summary>
    /// <typeparam name="T">Тип окна, которое создано через менеджер</typeparam>
    public void MaximizeWindow<T>()
        where T : Window
    {
        var window = GetWindow<T>();
        if (window is null)
            return;

        window.WindowState = WindowState.Maximized;
    }

    /// <summary>
    /// Закрывает окно и удаляет его из коллекции
    /// </summary>
    /// <typeparam name="T">Тип окна, которое создано через менеджер</typeparam>
    public void CloseWindow<T>()
        where T : Window
    {
        var window = GetWindow<T>();
        if (window is null)
            return;

        window.Close();
        _windows.Remove(typeof(T));
    }

    /// <summary>
    /// Проверяет, зарегистрировано ли окно
    /// </summary>
    /// <typeparam name="T">Тип окна, которое создано через менеджер</typeparam>
    /// <returns></returns>
    public bool IsWindowRegistered<T>()
        where T : Window
        => _windows.ContainsKey(typeof(T));

    #endregion

    #region Private methods

    /// <summary>
    /// Показывает переданное окно
    /// </summary>
    /// <typeparam name="T">Тип окна</typeparam>
    /// <param name="window">Окно для показа</param>
    /// <param name="showDialog">Если <b>true</b>, то <see cref="Window.ShowDialog()"/>, иначе <see cref="Window.Show()"/></param>
    private static void ShowWindow<T>(T window, bool showDialog = false)
        where T : Window
    {
        if (window.Visibility is Visibility.Visible)
            return;

        if (showDialog)
        {
            window.ShowDialog();
            return;
        }

        window.Show();
    }

    /// <summary>
    /// Регистрирует переданное окно
    /// </summary>
    /// <typeparam name="T">Тип окна</typeparam>
    /// <param name="window">Окно для регистрации и показа</param>
    /// <param name="isModern">Будет ли оно современным</param>
    /// <param name="showDialog">Если <b>true</b>, то <see cref="Window.ShowDialog()"/>, иначе <see cref="Window.Show()"/></param>
    /// <param name="owner">Владелец окна</param>
    /// <returns></returns>
    private T CreateWindow<T>(T window, bool isModern = true, bool showDialog = false, Window? owner = null)
        where T : Window
    {
        var windowType = typeof(T);
        WindowHelper.SetUseModernWindowStyle(window, isModern);
        window.Closed += (_, _) => _windows.Remove(windowType);
        window.Owner = owner;

        _windows[windowType] = window;
        ShowWindow(window, showDialog);
        return window;
    }

    #endregion

    #region Static methods

    /// <summary>
    /// Добавить перетаскивание экрана
    /// </summary>
    /// <param name="window">Окно</param>
    /// <param name="draggableElement">Перетаскиваемый элемент окна (<em>если <b>null</b>, то обработчик наложится на само окно</em>)</param>
    public static void AddDragMove(Window window, object? draggableElement = null)
    {
        WindowInteropHelper helper = new(window);
        if (draggableElement is null)
        {
            window.MouseLeftButtonDown += (_, _) => SendMessage(helper.Handle, 161, 2, 0);
            return;
        }

        var frameworkElement = (draggableElement as FrameworkElement)!;
        frameworkElement.MouseLeftButtonDown += (_, _) => SendMessage(helper.Handle, 161, 2, 0);
    }

    /// <summary>
    /// Показывает <see cref="MessageBox"/>
    /// </summary>
    /// <param name="text">Сообщение</param>
    /// <param name="caption">Заголовок</param>
    /// <param name="button">Кнопки</param>
    /// <param name="icon">Иконка</param>
    /// <returns>Ответ пользователя</returns>
    public static MessageBoxResult ShowMessageBox(
        string text,
        string caption,
        MessageBoxButton button = MessageBoxButton.OK,
        MessageBoxImage icon = MessageBoxImage.Information)
        => MessageBox.Show(text, caption, button, icon);

    #endregion
}
