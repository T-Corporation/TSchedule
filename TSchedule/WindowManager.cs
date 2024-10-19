using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace TSchedule;

public static class WindowManager
{
    // Коллекция для хранения окон
    private static readonly Dictionary<Type, Window> _windows = [];

    // Метод для создания и добавления окна в коллекцию
    public static T CreateWindow<T>(bool showDialog = false, Window? owner = null) where T : Window, new()
    {
        var windowType = typeof(T);
        if (_windows.TryGetValue(windowType, out Window? value))
            return (T)value;

        var window = new T { Owner = owner, };
        window.Closed += (_, _) => _windows.Remove(windowType);

        _windows[windowType] = window;
        ShowWindow<T>(showDialog);
        return window;
    }

    // Получение окна по его типу
    public static T? GetWindow<T>() where T : Window
    {
        var windowType = typeof(T);
        return _windows.TryGetValue(windowType, out var window)
            ? (T)window
            : null;
    }

    public static ObservableObject? GetViewModel<T>() where T : Window
    {
        var windowType = typeof(T);
        return GetWindow<T>()?.DataContext as ObservableObject;
    }

    public static T? As<T>(this ObservableObject viewModel) where T : ObservableObject
        => viewModel as T;

    // Показать окно (если оно скрыто)
    public static void ShowWindow<T>(bool showDialog = false) where T : Window, new()
    {
        var window = CreateWindow<T>();
        if (window.Visibility is Visibility.Visible)
            return;

        if (showDialog)
            window.ShowDialog();
        else
        window.Show();
    }

    // Скрыть окно
    public static void HideWindow<T>() where T : Window
    {
        var window = GetWindow<T>();
        if (window is null)
            return;

        window.Hide();
    }

    // Увеличить окно (сделать на весь экран)
    public static void MaximizeWindow<T>() where T : Window
    {
        var window = GetWindow<T>();
        if (window is null)
            return;

        window.WindowState = WindowState.Maximized;
    }

    // Закрыть окно и удалить его из коллекции
    public static void CloseWindow<T>() where T : Window
    {
        var window = GetWindow<T>();
        if (window is null)
            return;

        window.Close();
        _windows.Remove(typeof(T));
    }

    // Проверить, зарегистрировано ли окно
    public static bool IsWindowRegistered<T>() where T : Window => _windows.ContainsKey(typeof(T));

    public static MessageBoxResult ShowMessageBox(string text, string caption, MessageBoxButton button, MessageBoxImage icon)
        => MessageBox.Show(text, caption, button, icon);
}
