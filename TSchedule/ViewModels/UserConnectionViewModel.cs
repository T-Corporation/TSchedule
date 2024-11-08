using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Windows;
using TSchedule.Managers;
using TSchedule.Persistence;
using TSchedule.Views;

namespace TSchedule.ViewModels;

public partial class UserConnectionViewModel : ObservableObject
{
    [ObservableProperty]
    private string _connectionString = PreferencesManager.Default.GetConnectionString() ?? string.Empty;

    [RelayCommand]
    private async Task Migrate()
    {
        if (string.IsNullOrEmpty(ConnectionString))
        {
            WindowManager.ShowMessageBox(
                "Заполните строку подключения",
                "Предупреждение",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        if (!ConnectionString.Contains("TrustServerCertificate=True"))
        {
            WindowManager.ShowMessageBox(
                "Укажите доверенность сертификату с помощью TrustServerCertificate=True",
                "Предупреждение",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        if (WindowManager.ShowMessageBox(
            "Вы уверены, что хотите установить эту строку подключения?",
            "Подтверждение",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question)
            is not MessageBoxResult.Yes) return;

        ApplicationDbContext.ConnectionString = ConnectionString;

        try
        {
            await using ApplicationDbContext context = new(ConnectionString);
            await context.Database.MigrateAsync();
            await context.WarmUpAsync();

            WindowManager.ShowMessageBox(
                """
                Миграция успешно проведена!
                Пожалуйста, перезапустите приложение.
                """,
                "Успех!",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            PreferencesManager.Default.SetConnectionString(ConnectionString);
            PreferencesManager.Default.Save();

            WindowManager.Default.CloseWindow<UserConnectionWindow>();
        }
        catch (Exception ex)
        {
            WindowManager.ShowMessageBox(
                ex.Message,
                "Произошла ошибка при попытке миграции",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    private async Task Reset()
    {
        if (WindowManager.ShowMessageBox(
            "Вы уверены, что хотите сбросить строку подключения до заводских настроек?",
            "Подтверждение",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question)
            is not MessageBoxResult.Yes) return;

        ApplicationDbContext.ConnectionString = null;
        PreferencesManager.Default.ClearConnectionString();
        PreferencesManager.Default.Save();

        try
        {
            await using ApplicationDbContext context = new(ConnectionString);
            await context.WarmUpAsync();

            WindowManager.ShowMessageBox(
                """
                Сброс до заводских настроек прошёл успешно!
                Пожалуйста, перезапустите приложение.
                """,
                "Успех!",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            WindowManager.Default.CloseWindow<UserConnectionWindow>();
        }
        catch (Exception ex)
        {
            WindowManager.ShowMessageBox(
                ex.Message,
                "Произошла ошибка при попытке миграции",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
