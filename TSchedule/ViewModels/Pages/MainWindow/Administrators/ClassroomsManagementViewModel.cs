using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Modern.Controls;
using TSchedule.Managers;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Models;

namespace TSchedule.ViewModels.Pages.MainWindow.Administrators;

public partial class ClassroomsManagementViewModel : ObservableObject
{
    private readonly Lazy<IClassroomsService> ClassroomsService
        = new(ServiceManager.Default.GetRequiredService<IClassroomsService>);

    private Flyout AttachedFlyout { get; }

    private FrameworkElement Target { get; }

    [ObservableProperty]
    private ClassroomModel? _classroom;

    [ObservableProperty]
    private ObservableCollection<ClassroomModel> _classrooms = [];

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private string _classroomNumber = string.Empty;

    [ObservableProperty]
    private string _classroomType = string.Empty;

    private string[] ClassroomTypes =>
    [
        "Лабораторная", "Лекционная", "Семинарская", "Компьютерная", "Арт-студия", "Музыкальная", "Тренажерный зал"
    ];

    private ClassroomsManagementViewModel(
        IEnumerable<Classroom> classrooms,
        FrameworkElement element,
        Flyout flyout)
    {
        Target = element;
        AttachedFlyout = flyout;
        foreach (var classroom in classrooms)
            Classrooms.Add(classroom.ToModel());
    }

    public static async Task<ClassroomsManagementViewModel> CreateInstanceAsync(FrameworkElement element, Flyout flyout)
        => new(await ServiceManager.Default.GetRequiredService<IClassroomsService>()
            .GetAllClassrooms(), element, flyout);

    [RelayCommand]
    private void ShowFlyout()
    {
        IsEditing = false;
        ClassroomNumber = string.Empty;
        ClassroomType = ClassroomTypes[0];
        AttachedFlyout.ShowAt(Target);
    }

    [RelayCommand]
    private void ShowEditFlyout()
    {
        if (Classroom is null)
        {
            WindowManager.ShowMessageBox(
                text: "Не выбрана аудитория",
                caption: "Ошибка",
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error);
            return;
        }

        IsEditing = true;
        ClassroomNumber = Classroom.Number;
        ClassroomType = ClassroomTypes.FirstOrDefault(ct => ct == Classroom.Type)
            ?? ClassroomTypes[0];
        AttachedFlyout.ShowAt(Target);
    }

    [RelayCommand]
    private void HideFlyout() => AttachedFlyout.Hide();

    [RelayCommand]
    private async Task AddOrEdit()
    {
        ErrorMessage = string.Empty;
        const string pleaseFillField = "Пожалуйста, заполните поле \"{0}\"";

        if (string.IsNullOrEmpty(ClassroomNumber))
        {
            ErrorMessage = string.Format(pleaseFillField, "Номер кабинета");
            return;
        }

        if (string.IsNullOrEmpty(ClassroomType))
        {
            ErrorMessage = string.Format(pleaseFillField, "Тип аудитории");
            return;
        }

        Classroom classroom = new()
        {
            Type = ClassroomType,
            Number = ClassroomNumber
        };

        if (IsEditing)
        {
            if (Classroom is null)
            {
                WindowManager.ShowMessageBox(
                    text: "Не выбрана аудитория",
                    caption: "Ошибка",
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.Error);
                return;
            }

            try
            {
                classroom.Id = Classroom.Id;
                await ClassroomsService.Value.UpdateClassroom(classroom);
                var foundClassroom = Classrooms.First(c => c.Id == Classroom.Id);
                foundClassroom.Number = classroom.Number;
                foundClassroom.Type = classroom.Type;
                HideFlyout();
            }
            catch (UniqueException ue)
            {
                ErrorMessage = ue.Message;
            }
            
            return;
        }

        try
        {
            await ClassroomsService.Value.AddClassroom(classroom);
            Classrooms.Add(classroom.ToModel());
            HideFlyout();
        }
        catch (UniqueException ue)
        {
            ErrorMessage = ue.Message;
        }
    }

    [RelayCommand]
    private async void Delete()
    {
        if (Classroom is null)
        {
            WindowManager.ShowMessageBox(
                text: "Не выбрана аудитория",
                caption: "Ошибка",
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error);
            return;
        }

        if (WindowManager.ShowMessageBox(
            text: "Вы уверены, что хотите удалить аудиторию?",
            caption: "Подтверждение",
            button: MessageBoxButton.YesNo,
            icon: MessageBoxImage.Question) is not MessageBoxResult.Yes)
            return;

        await ClassroomsService.Value.RemoveClassroom(Classroom.Id);
        Classrooms.Remove(Classrooms.First(c => c.Id == Classroom.Id));
    }
}
