using TSchedule.Managers;
using TSchedule.Persistence;
using TSchedule.Persistence.Services;
using TSchedule.Persistence.Managers;
using TSchedule.Views;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Repositories;
using TSchedule.Persistence.Enums;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using System.Text;
using Windows.UI.ViewManagement;

namespace TSchedule;

public partial class App
{
    #pragma warning disable CA1416 // Проверка совместимости платформы
    private readonly UISettings uiSettings = new();

    private async void Application_Startup(object sender, StartupEventArgs e)
    {
        try
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            DispatcherUnhandledException += OnDispatcherUnhandledException;

            uiSettings.ColorValuesChanged += OnColorValuesChanged;
            CustomizationManager.Default.LoadStyleFromSettings()
                .ApplyThemeFromPreferences(uiSettings);

            ServiceManager.Default
                .AddSingleton<IUsersService, UsersService>(() => new UsersService(new UsersRepository()))
                .AddSingleton<IScheduleService, ScheduleService>(() => new ScheduleService(new ScheduleRepository()))
                .AddSingleton<IAnnouncementsService, AnnouncementsService>()
                .AddSingleton<IGroupsService, GroupsService>()
                .AddSingleton<IWeekDaysService, WeekDaysService>()
                .AddSingleton<ITeachersService, TeachersService>()
                .AddSingleton<IClassroomsService, ClassroomsService>()
                .AddSingleton<ISpecialtiesService, SpecialtiesService>()
                .AddSingleton<ISubjectsService, SubjectsService>();

            #if DEBUG
            PreferencesManager.Default.PrintValues();
            #endif

            if (Environment.GetCommandLineArgs().Contains("User-Connection"))
                WindowManager.Default.CreateWindow<UserConnectionWindow>(showDialog: true);

            await using ApplicationDbContext context = new(PreferencesManager.Default.GetConnectionString());
            await context.WarmUpAsync();

            await InitializeEntry();
        }
        catch (SqlException sqlEx)
        {
            #if DEBUG
            Debug.WriteLine("StackTrace:");
            Debug.WriteLine(sqlEx);
            #endif

            if (sqlEx.Message.Contains("network"))
            {
                if (WindowManager.ShowMessageBox(
                    "Сообщение: \"Отсутствует подключение к интернету.\"",
                    "Ошибка, перезагрузить приложение?",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Error) is MessageBoxResult.Yes)
                    RestartApplication();
                return;
            }

            throw;
        }
    }

    /// <summary>
    /// Инициализирует вход в приложение
    /// </summary>
    /// <returns>Задача</returns>
    public static async Task InitializeEntry()
    {
        var isLoggedIn = PreferencesManager.Default.IsLoggedIn();

        if (!isLoggedIn)
        {
            WindowManager.Default.CreateWindow<StartWindow>();
            return;
        }

        var id = PreferencesManager.Default.GetUserGuid();
        var role = PreferencesManager.Default.GetRole();

        var usersService = ServiceManager.Default.GetRequiredService<IUsersService>();
        await usersService.AuthenticateById(id, role switch
        {
            "Преподаватель" => Role.Преподаватель,
            "Администратор" => Role.Администратор,
            _ => Role.Гость
        });

        WindowManager.Default.CreateWindow<MainWindow>();
    }

    private void OnColorValuesChanged(UISettings sender, object args)
    {
        Dispatcher.Invoke(() => CustomizationManager.Default.ApplyThemeFromPreferences(uiSettings));
    }

    private void OnDispatcherUnhandledException(object _, DispatcherUnhandledExceptionEventArgs e)
    {
        HandleException(e.Exception);
        e.Handled = true;
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
            HandleException(ex);
    }

    private void HandleException(Exception exception)
    {
        var fullMessage = BuildExceptionMessage(exception);
        Debug.WriteLine(fullMessage);

        #if DEBUG
        Debug.WriteLine("StackTrace:");
        Debug.WriteLine(exception);
        #endif

        // Проверка, является ли исключение SqlException
        if (exception is SqlException sqlEx)
        {
            if (WindowManager.ShowMessageBox(
                $"""
                Сообщение: {sqlEx.Message}
                """,
                "Ошибка работы БД. Перезапустить приложение?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Error) is MessageBoxResult.Yes)
                RestartApplication();
            return;
        }
        
        // Логирование или обработка других исключений
        if (WindowManager.ShowMessageBox(
            $"""
            Вызванные исключения:
            {fullMessage}
            """,
            "Неожиданная ошибка. Закрыть приложение?",
            MessageBoxButton.YesNo,
            MessageBoxImage.Error) is MessageBoxResult.Yes)
            Shutdown();
    }

    private static string BuildExceptionMessage(Exception exception)
    {
        StringBuilder messageBuilder = new();
        var currentException = exception;
        int level = 1;

        while (currentException is not null)
        {
            messageBuilder.AppendLine($"(Уровень {level++}) {currentException.GetType().Name}: \"{currentException.Message}\"");
            currentException = currentException.InnerException;
        }

        return messageBuilder.ToString();
    }

    private void RestartApplication()
    {
        // Получение пути к текущему исполняемому файлу
        var exePath = Process.GetCurrentProcess().MainModule?.FileName;
        if (!string.IsNullOrEmpty(exePath))
            // Запуск нового процесса
            Process.Start(exePath);
        // Завершение текущего приложения
        Shutdown();
    }

    protected override void OnActivated(EventArgs e)
    {
        ServiceManager.Default.Dispose();
    }
}
