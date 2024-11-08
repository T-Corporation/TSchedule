using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Modern.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Models;
using WeekDay = TSchedule.Persistence.Entities.WeekDay;

namespace TSchedule.ViewModels;

public partial class TeachersManagementViewModel : ObservableObject
{
    public readonly ITeachersService TeachersService
        = ServiceManager.Default.GetRequiredService<ITeachersService>();

    public readonly IWeekDaysService DaysOfWeekService
        = ServiceManager.Default.GetRequiredService<IWeekDaysService>();

    public Flyout AttachedFlyout { get; }

    public FrameworkElement Target { get; }

    public string TeacherDateOfBirthPlaceholder => DateOnly.FromDateTime(DateTime.Now).ToString("dd.MM.yyyy");

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    [NotifyCanExecuteChangedFor(nameof(ShowEditFlyoutCommand))]
    private TeacherModel? _teacher;

    [ObservableProperty]
    private ObservableCollection<TeacherModel> _teachers = [];

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private string _teacherFullName = string.Empty;

    [ObservableProperty]
    private string _teacherUserName = string.Empty;

    [ObservableProperty]
    private string _teacherPassword = string.Empty;

    [ObservableProperty]
    private string? _teacherEmail = string.Empty;

    [ObservableProperty]
    private string? _teacherPhoneNumber = string.Empty;

    [ObservableProperty]
    private DateTime? _teacherDateOfBirth;

    [ObservableProperty]
    private ClassroomModel? _teacherClassroom;

    [ObservableProperty]
    private ObservableCollection<ClassroomModel> _classrooms = [];

    [ObservableProperty]
    private ObservableCollection<WeekDay> _daysOfWeek = [];

    [ObservableProperty]
    private ObservableCollection<TeacherPreferredTimeModel> _teacherPreferredTimes = [];

    [ObservableProperty]
    private ObservableCollection<SubjectModel> _subjects = [];

    [ObservableProperty]
    private SubjectModel? _teacherSubject = null!;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public TeachersManagementViewModel(
        IEnumerable<Teacher> teachers,
        IEnumerable<Classroom> classrooms,
        IEnumerable<Subject> subjects,
        IEnumerable<WeekDay> daysOfWeek,
        FrameworkElement element,
        Flyout flyout)
    {
        Target = element;
        AttachedFlyout = flyout;

        foreach (var teacher in teachers)
            Teachers.Add(teacher.ToModel());

        foreach (var classroom in classrooms)
            Classrooms.Add(classroom.ToModel());

        foreach (var subject in subjects)
            Subjects.Add(subject.ToModel());

        foreach (var dayOfWeek in daysOfWeek)
            DaysOfWeek.Add(dayOfWeek);
    }

    public static async Task<TeachersManagementViewModel> CreateInstanceAsync(FrameworkElement element, Flyout flyout)
        => new TeachersManagementViewModel(
            await ServiceManager.Default.GetRequiredService<ITeachersService>().GetAllTeachers(),
            await ServiceManager.Default.GetRequiredService<IClassroomsService>().GetAllClassrooms(),
            await ServiceManager.Default.GetRequiredService<ISubjectsService>().GetAllSubjects(),
            await ServiceManager.Default.GetRequiredService<IWeekDaysService>().GetAllDaysOfWeek(),
            element,
            flyout);

    [RelayCommand]
    private void ShowFlyout()
    {
        IsEditing = false;
        ErrorMessage = string.Empty;
        TeacherFullName = string.Empty;
        TeacherUserName = string.Empty;
        TeacherPassword = string.Empty;
        TeacherEmail = string.Empty;
        TeacherPhoneNumber = string.Empty;
        TeacherClassroom = null;
        TeacherDateOfBirth = null;
        TeacherSubject = null;

        TeacherPreferredTimes.Clear();
        foreach (var dayOfWeek in DaysOfWeek)
            TeacherPreferredTimes.Add(new TeacherPreferredTimeModel
            {
                DayOfWeek = dayOfWeek
            });

        AttachedFlyout.ShowAt(Target);
    }

    [RelayCommand(CanExecute = nameof(IsTeacherNotEmpty))]
    private void ShowEditFlyout()
    {
        IsEditing = true;
        ErrorMessage = string.Empty;
        if (Teacher is null)
        {
            WindowManager.ShowMessageBox(
                text: "Не выбран преподаватель",
                caption: "Ошибка",
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error);
            return;
        }

        TeacherFullName = Teacher.FullName;
        TeacherUserName = Teacher.UserName;
        TeacherPassword = Teacher.PasswordHash;
        TeacherEmail = Teacher.Email;
        TeacherPhoneNumber = Teacher.PhoneNumber;
        TeacherClassroom = Teacher.Classroom;
        TeacherSubject = Teacher.Subject;
        TeacherDateOfBirth = Teacher.DateOfBirth?.ToDateTime(TimeOnly.MinValue);

        TeacherPreferredTimes.Clear();

        for (int i = 0; i < DaysOfWeek.Count; i++)
        {
            // Если предпочтительное время не было заполнено, то Teacher.PreferredTimes.Length == 0.
            // Поэтому вылетает исключение ArgumentOutOfRangeException! Для обхода было создано собственное расширение.
            var preferredTime = Teacher.PreferredTimes.TryGetValue(i);
            TeacherPreferredTimes.Add(new TeacherPreferredTimeModel
            {
                Teacher = Teacher,
                DayOfWeek = DaysOfWeek[i],
                PreferredStart = preferredTime?.PreferredStart,
                PreferredEnd = preferredTime?.PreferredEnd
            });
        }

        AttachedFlyout.ShowAt(Target);
    }

    [RelayCommand]
    private void HideFlyout() => AttachedFlyout.Hide();

    private bool IsTeacherNotEmpty() => Teacher is not null;

    [RelayCommand]
    private async Task AddOrEdit()
    {
        ErrorMessage = string.Empty;
        const string pleaseFillField = "Пожалуйста, заполните поле \"{0}\"";

        if (string.IsNullOrEmpty(TeacherFullName))
        {
            ErrorMessage = string.Format(pleaseFillField, "ФИО");
            return;
        }

        if (TeacherFullName.Split(' ').Length < 3)
        {
            ErrorMessage = string.Format("Пожалуйста, напишите полное ФИО преподавателя");
            return;
        }

        if (string.IsNullOrEmpty(TeacherUserName))
        {
            ErrorMessage = string.Format(pleaseFillField, "Имя для входа");
            return;
        }

        if (string.IsNullOrEmpty(TeacherPassword))
        {
            ErrorMessage = string.Format(pleaseFillField, "Пароль для входа");
            return;
        }

        if (!PasswordManager.Default.IsPasswordSafe(TeacherPassword))
        {
            ErrorMessage = "Пароль не соответствует всем правилам. Наведите на поле пароля, чтобы увидеть правила";
            return;
        }

        var isEmailEmpty = string.IsNullOrEmpty(TeacherEmail);

        if (!isEmailEmpty && !EmailManager.Default.IsEmailValid(TeacherEmail!))
        {
            ErrorMessage = "Неверный или неподдерживаемый адрес почты";
            return;
        }

        var isPhoneNumberEmpty = string.IsNullOrEmpty(TeacherPhoneNumber);

        if (!isPhoneNumberEmpty && !PhoneManager.Default.IsPhoneNumberValid(TeacherPhoneNumber!))
        {
            ErrorMessage = "Неверный или неподдерживаемый номер телефона";
            return;
        }

        if (TeacherDateOfBirth is null)
        {
            ErrorMessage = string.Format(pleaseFillField, "Дата рождения");
            return;
        }

        if (TeacherClassroom is null)
        {
            ErrorMessage = string.Format(pleaseFillField, "Аудитория");
            return;
        }

        if (TeacherSubject is null)
        {
            ErrorMessage = string.Format(pleaseFillField, "Предмет");
            return;
        }

        // Получаем актуальный список преподавателей
        var allTeachers = await TeachersService.GetAllTeachers();

        // Проверяем, занята ли выбранная аудитория другим преподавателем
        if (allTeachers.Any(t => t.ClassroomId == TeacherClassroom.Id && t.Id != Teacher?.Id))
        {
            ErrorMessage = "Выбранная аудитория уже занята другим преподавателем";
            return;
        }

        // Проверяем, преподается ли выбранный предмет другим преподавателем
        if (allTeachers.Any(t => t.SubjectId == TeacherSubject.Id && t.Id != Teacher?.Id))
        {
            ErrorMessage = "Выбранный предмет уже ведётся другим преподавателем";
            return;
        }

        // Здесь у преподавателя пароль не хеширован - это для дальнейшей проверки внутри сервиса
        Teacher teacher = new()
        {
            FullName = TeacherFullName,
            UserName = TeacherUserName,
            PasswordHash = TeacherPassword,
            SubjectId = TeacherSubject.Id,
            ClassroomId = TeacherClassroom.Id,
            Email = isEmailEmpty ? null : TeacherEmail,
            PhoneNumber = isPhoneNumberEmpty ? null : TeacherPhoneNumber,
            DateOfBirth = DateOnly.FromDateTime(TeacherDateOfBirth.Value)
        };

        ObservableCollection<TeacherPreferredTime> preferredTimes = [.. TeacherPreferredTimes.Select(tpt => tpt.ToEntity())];

        if (IsEditing)
        {
            if (Teacher is null)
            {
                WindowManager.ShowMessageBox(
                    text: "Не выбран преподаватель",
                    caption: "Ошибка",
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.Error);
                return;
            }

            teacher.Id = Teacher.Id;
            foreach (var preferredTime in preferredTimes)
                preferredTime.TeacherId = teacher.Id;

            try
            {
                await TeachersService.UpdateTeacherWithPreferredTimes(teacher, preferredTimes);
                var foundTeacher = Teachers.First(t => t.Id == teacher.Id);
                foundTeacher.UserName = teacher.UserName;
                foundTeacher.FullName = teacher.FullName;
                foundTeacher.PhoneNumber = teacher.PhoneNumber;
                foundTeacher.Email = teacher.Email;
                foundTeacher.PreferredTimes = [.. preferredTimes.Select(tpt => tpt.ToModel())];
                foundTeacher.Classroom = TeacherClassroom;
                foundTeacher.Subject = TeacherSubject;
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
            await TeachersService.AddTeacherWithPreferredTimes(teacher, preferredTimes);
            teacher.Classroom = TeacherClassroom.ToEntity();
            teacher.Subject = TeacherSubject.ToEntity();
            Teachers.Add(teacher.ToModel());
            HideFlyout();
        }
        catch (UniqueException ue)
        {
            ErrorMessage = ue.Message;
        }
    }

    [RelayCommand(CanExecute = nameof(IsTeacherNotEmpty))]
    private async Task Delete()
    {
        if (Teacher is null)
        {
            WindowManager.ShowMessageBox(
                text: "Не выбран преподаватель",
                caption: "Ошибка",
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error);
            return;
        }

        if (WindowManager.ShowMessageBox(
            text: "Вы уверены, что хотите удалить преподавателя?",
            caption: "Подтверждение",
            button: MessageBoxButton.YesNo,
            icon: MessageBoxImage.Question) is not MessageBoxResult.Yes)
            return;

        await TeachersService.RemoveTeacherWithPreferredTimes(Teacher.Id);
        Teachers.Remove(Teachers.First(t => t.Id == Teacher.Id));
    }
}
