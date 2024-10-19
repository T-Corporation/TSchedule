﻿using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace TSchedule;

public static class WindowManager
{
    /// <summary>
    /// Коллекция зарегистрированных окон
    /// </summary>
    private static readonly Dictionary<Type, Window> _windows = [];

    /// <summary>
    /// Создаёт и регистрирует окно
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="showDialog"></param>
    /// <param name="owner"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Получает зарегистрированное окно по типу
    /// </summary>
    /// <typeparam name="T">Тип окна, которое создано через менеджер</typeparam>
    /// <returns>Если окно найдено, то возвращает его, иначе <b>null</b></returns>
    public static T? GetWindow<T>() where T : Window
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
    public static ObservableObject? GetViewModel<T>() where T : Window
        => GetWindow<T>()?.DataContext as ObservableObject;

    /// <summary>
    /// Преобразует DataContext к указанному типу
    /// </summary>
    /// <typeparam name="T">Тип DataContext</typeparam>
    /// <param name="viewModel">DataContext окна</param>
    /// <returns>DataContext, приведённый к типу <typeparamref name="T"/></returns>
    public static T? As<T>(this ObservableObject viewModel) where T : ObservableObject => viewModel as T;

    /// <summary>
    /// Показывает окно, если оно скрыто
    /// </summary>
    /// <typeparam name="T">Тип окна, которое создано через менеджер</typeparam>
    /// <param name="showDialog">Если <b>true</b>, то открывает в режиме диалога</param>
    public static void ShowWindow<T>(bool showDialog = false) where T : Window, new()
    {
        var window = CreateWindow<T>();
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
    /// Скрывает окно
    /// </summary>
    /// <typeparam name="T">Тип окна, которое создано через менеджер</typeparam>
    public static void HideWindow<T>() where T : Window
    {
        var window = GetWindow<T>();
        if (window is null)
            return;

        window.Hide();
    }

    /// <summary>
    /// Делает окно на весь экран
    /// </summary>
    /// <typeparam name="T">Тип окна, которое создано через менеджер</typeparam>
    public static void MaximizeWindow<T>() where T : Window
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
    public static void CloseWindow<T>() where T : Window
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
    public static bool IsWindowRegistered<T>() where T : Window => _windows.ContainsKey(typeof(T));

    /// <summary>
    /// Показывает <see cref="MessageBox"/>
    /// </summary>
    /// <param name="text">Сообщение</param>
    /// <param name="caption">Заголовок</param>
    /// <param name="button">Кнопки</param>
    /// <param name="icon">Иконка</param>
    /// <returns>Ответ пользователя</returns>
    public static MessageBoxResult ShowMessageBox(string text, string caption, MessageBoxButton button, MessageBoxImage icon)
        => MessageBox.Show(text, caption, button, icon);
}
