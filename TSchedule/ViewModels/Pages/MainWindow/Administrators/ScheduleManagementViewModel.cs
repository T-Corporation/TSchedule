using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Modern.Controls;
using System.Collections.ObjectModel;
using TSchedule.Extensions;
using TSchedule.Managers;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Models;

namespace TSchedule.ViewModels.Pages.MainWindow.Administrators;

public partial class ScheduleManagementViewModel : ObservableObject
{
    private const int TotalLessonsCount = 6;

    private static readonly IScheduleService ScheduleService = 
        ServiceManager.Default.GetRequiredService<IScheduleService>();

    public static readonly ISubjectsService SubjectsService = 
        ServiceManager.Default.GetRequiredService<ISubjectsService>();

    public static readonly ITeachersService TeachersService = 
        ServiceManager.Default.GetRequiredService<ITeachersService>();

    private static readonly IClassroomsService ClassroomsService = 
        ServiceManager.Default.GetRequiredService<IClassroomsService>();

    public Flyout Flyout { get; }

    public static readonly byte CurrentSemester = (byte)(DateTime.Now.Month is >= 9 and <= 12 ? 1 : 2);

    public static readonly short CurrentYear = (short)DateTime.Now.Year;

    // Список данных для числителя и знаменателя
    [ObservableProperty]
    private ObservableCollection<LessonScheduleModel> _numeratorDailySchedule = [];

    [ObservableProperty]
    private ObservableCollection<LessonScheduleModel> _denominatorDailySchedule = [];

    // Выбранные значения
    [ObservableProperty]
    private ScheduleModel? _selectedSchedule;

    partial void OnSelectedScheduleChanged(ScheduleModel? value)
    {
        // Очистить поля, если нет выбранного расписания (создание нового расписания)
        if (value is null)
        {
            SelectedClassroom = null;
            SelectedSubject = null;
            SelectedTeacher = null;
            SelectedDayOfWeek = null;
            SelectedLesson = null;
            IsDenominator = false;
            return;
        }
        
        // Заполнить поля, если выбранное расписание уже существует (редактирование)
        SelectedClassroom = value.Teacher?.Classroom;
        SelectedSubject = value.Teacher?.Subject;
        SelectedTeacher = value.Teacher;
        SelectedDayOfWeek = value.WeekDay;
        SelectedLesson = value.Lesson;
        IsDenominator = value.IsDenominator;
    }

    [ObservableProperty]
    private GroupModel? _selectedGroup;

    [ObservableProperty]
    private byte _selectedSemester;

    [ObservableProperty]
    private short _selectedYear;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private SubjectModel? _selectedSubject;

    partial void OnSelectedSubjectChanged(SubjectModel? value)
    {
        if (value is null)
        {
            SelectedTeacher = null;
            SelectedClassroom = null;
            return;
        }

        _ = UpdateComboboxes(value);
    }
    
    public async Task UpdateComboboxes(SubjectModel value)
    {
        SelectedTeacher = (await TeachersService.GetTeacherBySubjectId(value.Id))?
            .ToModel();

        SelectedClassroom = SelectedTeacher?.Classroom;
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private TeacherModel? _selectedTeacher;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private ClassroomModel? _selectedClassroom;

    [ObservableProperty]
    private WeekDay? _selectedDayOfWeek;

    [ObservableProperty]
    public LessonModel? _selectedLesson;

    [ObservableProperty]
    private bool _isDenominator;

    [ObservableProperty]
    private ObservableCollection<SubjectModel> _subjects = [];

    [ObservableProperty]
    private ObservableCollection<TeacherModel> _teachers = [];

    [ObservableProperty]
    private ObservableCollection<ClassroomModel> _classrooms = [];

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public ScheduleManagementViewModel(
        Flyout flyout,
        GroupModel selectedGroup,
        byte selectedSemester,
        short selectedYear,
        IEnumerable<Subject> subjects,
        IEnumerable<Teacher> teachers,
        IEnumerable<Classroom> classrooms,
        IEnumerable<Schedule> numeratorSchedules,
        IEnumerable<Schedule> denominatorSchedules)
    {
        // Группируем расписания по дням и создаем DailyScheduleModel

        Flyout = flyout;
        SelectedGroup = selectedGroup;
        SelectedSemester = selectedSemester;
        SelectedYear = selectedYear;

        foreach (var subject in subjects)
            Subjects.Add(subject.ToModel());

        foreach (var teacher in teachers)
            Teachers.Add(teacher.ToModel());

        foreach (var classroom in classrooms)
            Classrooms.Add(classroom.ToModel());

        PopulateLessonSchedules(numeratorSchedules, NumeratorDailySchedule);
        PopulateLessonSchedules(denominatorSchedules, DenominatorDailySchedule);
    }

    public static async Task<ScheduleManagementViewModel> CreateInstanceAsync(
        Flyout flyout,
        GroupModel selectedGroup,
        byte selectedSemester,
        short selectedYear)
        => new(
            flyout,
            selectedGroup,
            selectedSemester,
            selectedYear,
            await SubjectsService.GetSubjectsBySpecialtyId(selectedGroup.Specialty!.Id),
            await TeachersService.GetAllTeachers(),
            await ClassroomsService.GetAllClassrooms(),
            await ScheduleService.GetSchedules(false),
            await ScheduleService.GetSchedules(true));

    private void PopulateLessonSchedules(
        IEnumerable<Schedule> schedules,
        ObservableCollection<LessonScheduleModel> observableCollection)
    {
        var groupedSchedules = schedules
            .Where(s => s.GroupId == SelectedGroup!.Id && s.Year == SelectedYear && s.Semester == SelectedSemester)
            .GroupBy(s => s.LessonId)
            .ToDictionary(g => g.Key, g => g.ToList());

        for (int lessonNumber = 1; lessonNumber <= TotalLessonsCount; lessonNumber++)
        {
            var lessonSchedule = new LessonScheduleModel
            {
                Lesson = new LessonModel { Id = lessonNumber }
            };

            if (groupedSchedules.TryGetValue(lessonNumber, out var schedulesForLesson))
            {
                lessonSchedule.Monday = schedulesForLesson.FirstOrDefault(s => s.WeekDay!.Id == 1)?.ToModel();
                lessonSchedule.Tuesday = schedulesForLesson.FirstOrDefault(s => s.WeekDay!.Id == 2)?.ToModel();
                lessonSchedule.Wednesday = schedulesForLesson.FirstOrDefault(s => s.WeekDay!.Id == 3)?.ToModel();
                lessonSchedule.Thursday = schedulesForLesson.FirstOrDefault(s => s.WeekDay!.Id == 4)?.ToModel();
                lessonSchedule.Friday = schedulesForLesson.FirstOrDefault(s => s.WeekDay!.Id == 5)?.ToModel();
                lessonSchedule.Saturday = schedulesForLesson.FirstOrDefault(s => s.WeekDay!.Id == 6)?.ToModel();
                lessonSchedule.Sunday = schedulesForLesson.FirstOrDefault(s => s.WeekDay!.Id == 7)?.ToModel();
            }

            observableCollection.Add(lessonSchedule);
        }
    }

    private bool IsFieldsNotEmpty() => SelectedClassroom is not null
        && SelectedSubject is not null
        && SelectedTeacher is not null;

    /// <summary>
    /// Команда для сохранения расписания. Проверяет заполненность полей и валидирует расписание перед сохранением.
    /// </summary>
    [RelayCommand(CanExecute = nameof(IsFieldsNotEmpty))]
    private async Task Save()
    {
        if (!ValidateSchedule(out var validationErrorMessage))
        {
            ErrorMessage = validationErrorMessage;
            return;
        }

        var scheduleCollection = IsDenominator ? DenominatorDailySchedule : NumeratorDailySchedule;

        // Создаем или обновляем расписание
        if (SelectedSchedule is null)
        {
            // Создание нового расписания
            SelectedSchedule = new ScheduleModel
            {
                WeekDay = SelectedDayOfWeek,
                Teacher = SelectedTeacher,
                Semester = SelectedSemester,
                Year = SelectedYear,
                IsDenominator = IsDenominator,
                Group = SelectedGroup,
                Lesson = SelectedLesson
            };

            // Добавление расписания в коллекцию и БД
            SelectedSchedule.Id = (await ScheduleService.AddSchedule(SelectedSchedule.ToEntity())).Id;
            UpdateLessonInCollection(scheduleCollection, SelectedSchedule);
            Flyout.Hide();
            return;
        }
        
        // Обновление существующего расписания
        UpdateExistingSchedule(SelectedSchedule);

        // Обновление в БД
        await ScheduleService.UpdateSchedule(SelectedSchedule.ToEntity());

        // Обновление расписания в коллекции
        UpdateLessonInCollection(scheduleCollection, SelectedSchedule);
        Flyout.Hide();
    }

    /// <summary>
    /// Создает объект <see cref="LessonScheduleModel"/> на основе переданного расписания.
    /// </summary>
    /// <param name="schedule">Расписание одного занятия.</param>
    /// <returns>Объект LessonScheduleModel, содержащий расписание для конкретного дня.</returns>
    private static LessonScheduleModel CreateLessonScheduleModel(ScheduleModel schedule)
    {
        var lessonSchedule = new LessonScheduleModel { Lesson = schedule.Lesson! };
        switch (schedule.WeekDay?.Id)
        {
            case 1: lessonSchedule.Monday = schedule; break;
            case 2: lessonSchedule.Tuesday = schedule; break;
            case 3: lessonSchedule.Wednesday = schedule; break;
            case 4: lessonSchedule.Thursday = schedule; break;
            case 5: lessonSchedule.Friday = schedule; break;
            case 6: lessonSchedule.Saturday = schedule; break;
            case 7: lessonSchedule.Sunday = schedule; break;
        }
        return lessonSchedule;
    }

    /// <summary>
    /// Обновляет поля выбранного расписания на основе текущих данных.
    /// </summary>
    /// <param name="schedule">Расписание, которое необходимо обновить.</param>
    private void UpdateExistingSchedule(ScheduleModel schedule)
    {
        schedule.WeekDay = SelectedDayOfWeek;
        schedule.Teacher = SelectedTeacher;
        schedule.Semester = SelectedSemester;
        schedule.Year = SelectedYear;
        schedule.IsDenominator = IsDenominator;
        schedule.Group = SelectedGroup;
        schedule.Lesson = SelectedLesson;
    }

    /// <summary>
    /// Обновляет объект расписания в указанной коллекции расписаний.
    /// </summary>
    /// <param name="scheduleCollection">Коллекция расписаний.</param>
    /// <param name="schedule">Расписание, которое необходимо обновить.</param>
    private static void UpdateLessonInCollection(
        ObservableCollection<LessonScheduleModel> scheduleCollection, 
        ScheduleModel schedule)
    {
        var lessonSchedule = scheduleCollection.FirstOrDefault(
            ls => ls.Lesson?.Id == schedule.Lesson?.Id);

        if (lessonSchedule is not null)
            switch (schedule.WeekDay?.Id)
            {
                case 1: lessonSchedule.Monday = schedule; break;
                case 2: lessonSchedule.Tuesday = schedule; break;
                case 3: lessonSchedule.Wednesday = schedule; break;
                case 4: lessonSchedule.Thursday = schedule; break;
                case 5: lessonSchedule.Friday = schedule; break;
                case 6: lessonSchedule.Saturday = schedule; break;
                case 7: lessonSchedule.Sunday = schedule; break;
            }
    }

    /// <summary>
    /// Проверяет корректность расписания на отсутствие конфликтов и соответствие требованиям.
    /// </summary>
    /// <param name="errorMessage">Сообщение об ошибке, если проверка не пройдена.</param>
    /// <returns>Истина, если расписание корректно, иначе ложь.</returns>
    private bool ValidateSchedule(out string errorMessage)
    {
        errorMessage = string.Empty;

        // Проверка на отсутствие пересечения занятий одного преподавателя
        if (IsTeacherBusy(SelectedTeacher!, SelectedDayOfWeek!, SelectedLesson!, IsDenominator))
        {
            errorMessage = "Преподаватель уже занят в это время.";
            return false;
        }

        // Проверка на доступность аудитории
        if (IsClassroomOccupied(SelectedClassroom!, SelectedDayOfWeek!, SelectedLesson!, IsDenominator))
        {
            errorMessage = "Аудитория уже занята в это время.";
            return false;
        }

        // Проверка на соответствие требованиям учебной нагрузки
        if (!MeetsScheduleRequirements(SelectedSubject!))
        {
            errorMessage = "Кол-во часов превышает учебную нагрузку.";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Проверяет, не превышает ли суммарное количество часов по предмету допустимую учебную нагрузку.
    /// </summary>
    /// <param name="subject">Предмет, для которого проверяется нагрузка.</param>
    /// <returns>Истина, если нагрузка не превышена, иначе ложь.</returns>
    private bool MeetsScheduleRequirements(SubjectModel subject)
    {
        int weeklyHours = subject.WeeklyHours;
        int currentHours = CalculateCurrentWeeklyHours(subject.Id);

        // Проверка, что добавление занятия не превысит допустимое количество часов
        if (++currentHours > weeklyHours)
            return false;

        return true;
    }

    /// <summary>
    /// Рассчитывает текущее количество часов для предмета за неделю, учитывая числитель и знаменатель.
    /// </summary>
    /// <param name="subjectId">Идентификатор предмета.</param>
    /// <returns>Среднее количество часов в неделю для данного предмета.</returns>
    private int CalculateCurrentWeeklyHours(int subjectId)
    {
        int numeratorHours = CalculateHoursForSchedule(NumeratorDailySchedule, subjectId);
        int denominatorHours = CalculateHoursForSchedule(DenominatorDailySchedule, subjectId);

        // Среднее количество часов на неделю
        return (numeratorHours + denominatorHours) / 2;
    }

    /// <summary>
    /// Вычисляет общее количество часов по расписанию для указанного предмета.
    /// </summary>
    /// <param name="schedule">Коллекция расписаний.</param>
    /// <param name="subjectId">Идентификатор предмета.</param>
    /// <returns>Общее количество часов для указанного предмета.</returns>
    private static int CalculateHoursForSchedule(ObservableCollection<LessonScheduleModel> schedule, int subjectId)
    {
        int totalHours = 0;

        foreach (var daySchedule in schedule)
            foreach (var lesson in daySchedule.GetLessons()) // Предполагается метод GetLessons(), возвращающий расписания по всем дням недели
                if (lesson is not null && lesson.Teacher?.Subject?.Id == subjectId)
                    totalHours += 2;

        return totalHours;
    }

    /// <summary>
    /// Проверяет, занят ли преподаватель в указанное время.
    /// </summary>
    /// <param name="teacher">Модель преподавателя.</param>
    /// <param name="dayOfWeek">День недели.</param>
    /// <param name="lesson">Занятие.</param>
    /// <param name="isDenominator">Флаг, указывающий на знаменатель или числитель.</param>
    /// <returns>Истина, если преподаватель занят, иначе ложь.</returns>
    private bool IsTeacherBusy(TeacherModel teacher, WeekDay dayOfWeek, LessonModel lesson, bool isDenominator)
    {
        var schedules = isDenominator ? DenominatorDailySchedule : NumeratorDailySchedule;
        foreach (var daySchedule in schedules)
        {
            if (daySchedule.Lesson is null || daySchedule.Lesson.Id != lesson.Id)
                continue;

            var lessonSchedule = GetLessonScheduleByDayOfWeek(daySchedule, dayOfWeek);
            if (lessonSchedule is not null
                && lessonSchedule.IsDenominator == isDenominator
                && lessonSchedule.Teacher?.Id == teacher.Id)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Получает расписание занятия для конкретного дня недели.
    /// </summary>
    /// <param name="daySchedule">Модель расписания для дня.</param>
    /// <param name="dayOfWeek">День недели.</param>
    /// <returns>Расписание для указанного дня недели.</returns>
    private static ScheduleModel? GetLessonScheduleByDayOfWeek(LessonScheduleModel daySchedule, WeekDay dayOfWeek) => dayOfWeek.Id switch
    {
        1 => daySchedule.Monday,
        2 => daySchedule.Tuesday,
        3 => daySchedule.Wednesday,
        4 => daySchedule.Thursday,
        5 => daySchedule.Friday,
        6 => daySchedule.Saturday,
        7 => daySchedule.Sunday,
        _ => null
    };

    /// <summary>
    /// Проверяет, занята ли аудитория в указанное время.
    /// </summary>
    /// <param name="classroom">Модель аудитории.</param>
    /// <param name="dayOfWeek">День недели.</param>
    /// <param name="lesson">Занятие.</param>
    /// <param name="isDenominator">Флаг, указывающий на знаменатель или числитель.</param>
    /// <returns>Истина, если аудитория занята, иначе ложь.</returns>
    private bool IsClassroomOccupied(ClassroomModel classroom, WeekDay dayOfWeek, LessonModel lesson, bool isDenominator)
    {
        var schedules = isDenominator ? DenominatorDailySchedule : NumeratorDailySchedule;
        foreach (var daySchedule in schedules)
        {
            if (daySchedule.Lesson is null || daySchedule.Lesson.Id != lesson.Id)
                continue;

            var lessonSchedule = GetLessonScheduleByDayOfWeek(daySchedule, dayOfWeek);
            if (lessonSchedule is not null && lessonSchedule.Teacher?.Classroom?.Id == classroom.Id)
                return true;
        }
        return false;
    }

    private static void SetToNullLessonInCollection(
        ObservableCollection<LessonScheduleModel> scheduleCollection,
        ScheduleModel schedule)
    {
        var lessonSchedule = scheduleCollection.FirstOrDefault(ls => ls.Lesson?.Id == schedule.Lesson?.Id);
        if (lessonSchedule is not null)
        {
            switch (schedule.WeekDay?.Id)
            {
                case 1: lessonSchedule.Monday = null; break;
                case 2: lessonSchedule.Tuesday = null; break;
                case 3: lessonSchedule.Wednesday = null; break;
                case 4: lessonSchedule.Thursday = null; break;
                case 5: lessonSchedule.Friday = null; break;
                case 6: lessonSchedule.Saturday = null; break;
                case 7: lessonSchedule.Sunday = null; break;
            }
        }
    }

    [RelayCommand]
    private async Task Delete()
    {
        await ScheduleService.RemoveSchedule(SelectedSchedule!.Id);
        var scheduleCollection = IsDenominator ? DenominatorDailySchedule : NumeratorDailySchedule;
        SetToNullLessonInCollection(scheduleCollection, SelectedSchedule);
    }

    [RelayCommand]
    private void OpenWizard(string wizardString)
    {
        if (SelectedGroup is null) return;

        WindowManager.Default.CreateWindowWithParameters<Views.ImportExportWindow>(
            showDialog: true,
            parameters:
            [
                wizardString switch
                {
                    "Import" => WizardType.Import,
                    _ => WizardType.Export
                },
                new ExcelManager.LessonSchedules(SelectedGroup, SelectedSemester, SelectedYear, NumeratorDailySchedule, DenominatorDailySchedule)
            ]);
    }

    public async Task UpdateSchedules(
        ICollection<LessonScheduleModel> numeratorDailySchedule,
        ICollection<LessonScheduleModel> denominatorDailySchedule)
    {
        foreach (var lessonSchedule in NumeratorDailySchedule.Concat(DenominatorDailySchedule))
            foreach (var schedule in lessonSchedule.GetLessons())
                if (schedule is not null)
                    await ScheduleService.RemoveSchedule(schedule.Id);

        foreach (var lessonSchedule in numeratorDailySchedule.Concat(denominatorDailySchedule))
            foreach (var schedule in lessonSchedule.GetLessons())
                if (schedule is not null)
                {
                    schedule.Semester = SelectedSemester;
                    schedule.Year = SelectedYear;
                    await ScheduleService.AddSchedule(schedule.ToEntity());
                }

        NumeratorDailySchedule = [.. numeratorDailySchedule];
        DenominatorDailySchedule = [.. denominatorDailySchedule];
    }
}
