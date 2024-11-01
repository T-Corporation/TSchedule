using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Modern.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using TSchedule.Extensions;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Models;

namespace TSchedule.ViewModels.Pages.MainWindow.Administrators;

public partial class ScheduleManagementViewModel : ObservableObject
{
    public WeekDay[] Days =>
    [
        WeekDays.Monday, WeekDays.Tuesday, WeekDays.Wednesday, WeekDays.Thursday, WeekDays.Friday, WeekDays.Sunday, WeekDays.Saturday
    ];

    private const int TotalLessonsCount = 6;

    private static readonly IScheduleService ScheduleService
        = ServiceManager.Default.GetRequiredService<IScheduleService>();

    private static readonly ISubjectsService SubjectsService
        = ServiceManager.Default.GetRequiredService<ISubjectsService>();

    private static readonly ITeachersService TeachersService
        = ServiceManager.Default.GetRequiredService<ITeachersService>();

    private static readonly IClassroomsService ClassroomsService
        = ServiceManager.Default.GetRequiredService<IClassroomsService>();

    private static readonly byte CurrentSemester = (byte)(DateTime.Now.Month is >= 9 and <= 12 ? 1 : 2);

    private static readonly short CurrentYear = (short)DateTime.Now.Year;

    public Flyout Flyout { get; }
    public FrameworkElement Target { get; set; } = null!;

    // Список данных для числителя и знаменателя
    [ObservableProperty]
    private ObservableCollection<ScheduleModel> _numeratorSchedule = [];

    [ObservableProperty]
    private ObservableCollection<ScheduleModel> _denominatorSchedule = [];

    // Выбранные значения
    [ObservableProperty]
    private ScheduleModel? _selectedSchedule;

    partial void OnSelectedScheduleChanged(ScheduleModel? value)
    {
        if (value is null || value.Id == 0)
        {
            // Очистить поля, если нет выбранного расписания (создание нового расписания)
            SelectedClassroom = null;
            SelectedSubject = null;
            SelectedTeacher = null;
            SelectedDayOfWeek = WeekDays.Monday;
            SelectedLessonNumber = 0;
            IsDenominator = false;
        }
        else
        {
            // Заполнить поля, если выбранное расписание уже существует (редактирование)
            SelectedClassroom = value.Classroom;
            SelectedSubject = value.Subject;
            SelectedTeacher = value.Teacher;
            SelectedDayOfWeek = value.WeekDay!;
            SelectedLessonNumber = value.LessonNumber;
            IsDenominator = value.IsDenominator;
        }
    }

    [ObservableProperty]
    private GroupModel? _selectedGroup;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private SubjectModel? _selectedSubject;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private TeacherModel? _selectedTeacher;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private ClassroomModel? _selectedClassroom;

    [ObservableProperty]
    private bool _isDenominator;

    [ObservableProperty]
    private ObservableCollection<SubjectModel> _subjects = [];

    [ObservableProperty]
    private ObservableCollection<TeacherModel> _teachers = [];

    [ObservableProperty]
    private ObservableCollection<ClassroomModel> _classrooms = [];

    [ObservableProperty]
    private WeekDay _selectedDayOfWeek = null!;

    [ObservableProperty]
    public byte _selectedLessonNumber;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public ScheduleManagementViewModel(
        GroupModel selectedGroup,
        IEnumerable<Subject> subjects,
        IEnumerable<Teacher> teachers,
        IEnumerable<Classroom> classrooms,
        IEnumerable<Schedule> numeratorSchedules,
        IEnumerable<Schedule> denominatorSchedules,
        Flyout flyout,
        FrameworkElement target)
    {
        Flyout = flyout;
        Target = target;
        SelectedGroup = selectedGroup;

        numeratorSchedules = numeratorSchedules.Where(s => s.GroupId == SelectedGroup.Id
            && s.Year == CurrentYear
            && s.Semester == CurrentSemester);

        denominatorSchedules = denominatorSchedules.Where(s => s.GroupId == SelectedGroup.Id
            && s.Year == CurrentYear
            && s.Semester == CurrentSemester);

        foreach (var numeratorSchedule in numeratorSchedules)
            NumeratorSchedule.Add(numeratorSchedule.ToModel());

        foreach (var denominatorSchedule in denominatorSchedules)
            DenominatorSchedule.Add(denominatorSchedule.ToModel());

        AddPlaceholderRows(NumeratorSchedule);
        AddPlaceholderRows(DenominatorSchedule);

        foreach (var subject in subjects)
            Subjects.Add(subject.ToModel());

        foreach (var teacher in teachers)
            Teachers.Add(teacher.ToModel());

        foreach (var classroom in classrooms)
            Classrooms.Add(classroom.ToModel());
    }

    public static async Task<ScheduleManagementViewModel> CreateInstanceAsync(GroupModel selectedGroup, Flyout flyout, FrameworkElement target)
        => new(
            selectedGroup,
            await SubjectsService.GetAllSubjects(),
            await TeachersService.GetAllTeachers(),
            await ClassroomsService.GetAllClassrooms(),
            await ScheduleService.GetSchedules(false),
            await ScheduleService.GetSchedules(true),
            flyout,
            target);

    private void AddPlaceholderRows(ObservableCollection<ScheduleModel> scheduleCollection)
{
    var existingLessons = scheduleCollection
        .Where(s => s.WeekDay != null)
        .GroupBy(s => s.WeekDay)
        .ToDictionary(g => g.Key, g => g.Select(s => s.LessonNumber).ToHashSet());

    foreach (var day in Days)
    {
        for (int lesson = 1; lesson <= TotalLessonsCount; lesson++)
        {
            if (!existingLessons.TryGetValue(day, out var lessons) || !lessons.Contains((byte)lesson))
            {
                var newSchedule = new ScheduleModel { WeekDay = day, LessonNumber = (byte)lesson };
                scheduleCollection.Add(newSchedule);
            }
        }
    }

    // Сортировка расписания по дню недели и номеру урока
    var sortedSchedule = scheduleCollection
        .Where(s => s.WeekDay != null) // Отфильтровывать только варианты с непроживаемым значением
        .OrderBy(s => s.WeekDay) // Преобразуем WeekDay в int, если не null
        .ThenBy(s => s.LessonNumber)
        .ToList();
        
    scheduleCollection.Clear();
    foreach (var item in sortedSchedule)
    {
        scheduleCollection.Add(item);
    }
}

    [RelayCommand]
    private void ShowFlyout(string info)
    {
        var parts = info.Split(';');

        if (parts.Length != 2 || !byte.TryParse(parts[1], out var isDenominator))
            throw new FormatException("Поддерживается только формат \"День недели (ПН);0 или 1 (Числитель=0, Знаменатель=1)\"");

        SelectedDayOfWeek = parts[0] switch
        {
            "ПН" => WeekDays.Monday,
            "ВТ" => WeekDays.Tuesday,
            "СР" => WeekDays.Wednesday,
            "ЧТ" => WeekDays.Thursday,
            "ПТ" => WeekDays.Friday,
            "СБ" => WeekDays.Sunday,
            "ВС" => WeekDays.Saturday,
            _ => throw new ArgumentException("Неправильный день недели")
        };

        if (isDenominator == 1)
        {
            IsDenominator = true;
        }
        else
        {
            IsDenominator = false;
        }

        // Проверьте, есть ли уже расписание для этой ячейки
        var scheduleForCell = NumeratorSchedule.Concat(DenominatorSchedule)
            .FirstOrDefault(s => s.WeekDay == SelectedDayOfWeek && s.LessonNumber == SelectedLessonNumber && s.IsDenominator == IsDenominator);
    
        if (scheduleForCell is not null)
        {
            SelectedSchedule = scheduleForCell; // Заполнение значениями из существующего расписания
        }
        else
        {
            SelectedSchedule = new ScheduleModel(); // Очистить выбор, если <null>
        }

        Flyout.ShowAttachedFlyout(Target);
    }

    private bool IsFieldsNotEmpty() => SelectedClassroom is not null
        && SelectedSubject is not null
        && SelectedTeacher is not null;

    [RelayCommand(CanExecute = nameof(IsFieldsNotEmpty))]
    private void Save(string day)
    {
        if (SelectedSchedule is null || SelectedSchedule.Id == 0)
        {
            SelectedSchedule = new ScheduleModel
            {
                WeekDay = SelectedDayOfWeek,
                Subject = SelectedSubject,
                Teacher = SelectedTeacher,
                Classroom = SelectedClassroom,
                Semester = CurrentSemester,
                Year = CurrentYear,
                IsDenominator = IsDenominator,
                Group = SelectedGroup,
                LessonNumber = SelectedLessonNumber
            };

            // Добавить расписание в коллекцию (Numerator или Denominator)
            var targetCollection = IsDenominator ? DenominatorSchedule : NumeratorSchedule;
            targetCollection.Add(SelectedSchedule);
        }
        else
        {
            // Обновить существующее расписание
            SelectedSchedule.WeekDay = SelectedDayOfWeek;
            SelectedSchedule.Subject = SelectedSubject;
            SelectedSchedule.Teacher = SelectedTeacher;
            SelectedSchedule.Classroom = SelectedClassroom;
            SelectedSchedule.Semester = CurrentSemester;
            SelectedSchedule.Year = CurrentYear;
            SelectedSchedule.IsDenominator = IsDenominator;
            SelectedSchedule.Group = SelectedGroup;
            SelectedSchedule.LessonNumber = SelectedLessonNumber;
        }

        var schedules = NumeratorSchedule.Concat(DenominatorSchedule);

        // 1. Проверка на пересечение занятий одного преподавателя
        if (schedules.Any(s => 
            s.Teacher?.Id == SelectedTeacher?.Id &&
            s.WeekDay == SelectedDayOfWeek &&
            s.LessonNumber == SelectedSchedule.LessonNumber &&
            s.IsDenominator == IsDenominator))
        {
            ErrorMessage = "Преподаватель уже занят в это время.";
            return;
        }

        // 2. Проверка на доступность аудитории
        if (schedules.Any(s =>
            s.Classroom?.Id == SelectedClassroom?.Id &&
            s.WeekDay == SelectedDayOfWeek &&
            s.LessonNumber == SelectedSchedule.LessonNumber &&
            s.IsDenominator == IsDenominator))
        {
            ErrorMessage = "Аудитория занята в указанное время.";
            return;
        }

        // 3. Проверка соответствия предмета преподавателя и выбранного предмета
        if (SelectedTeacher!.Subject!.Id != SelectedSubject!.Id)
        {
            ErrorMessage = "Указанный преподаватель не может вести выбранный предмет.";
            return;
        }

        switch (SelectedSchedule.LessonNumber)
        {
            case 1:
                SelectedSchedule.StartTime = TimeOnly.Parse("8:30");
                SelectedSchedule.EndTime = TimeOnly.Parse("10:05");
                break;
            case 2:
                SelectedSchedule.StartTime = TimeOnly.Parse("10:15");
                SelectedSchedule.EndTime = TimeOnly.Parse("11:50");
                break;
            case 3:
                SelectedSchedule.StartTime = TimeOnly.Parse("12:30");
                SelectedSchedule.EndTime = TimeOnly.Parse("14:05");
                break;
            case 4:
                SelectedSchedule.StartTime = TimeOnly.Parse("14:15");
                SelectedSchedule.EndTime = TimeOnly.Parse("15:50");
                break;
            case 5:
                SelectedSchedule.StartTime = TimeOnly.Parse("16:00");
                SelectedSchedule.EndTime = TimeOnly.Parse("17:35");
                break;
            case 6:
                SelectedSchedule.StartTime = TimeOnly.Parse("17:45");
                SelectedSchedule.EndTime = TimeOnly.Parse("19:20");
                break;
            default:
                break;
        }

        Flyout.Hide();
    }
}
