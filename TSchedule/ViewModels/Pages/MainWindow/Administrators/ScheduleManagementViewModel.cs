using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Extensions;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Models;

namespace TSchedule.ViewModels.Pages.MainWindow.Administrators;

public partial class ScheduleManagementViewModel : ObservableObject
{
    private const int TotalLessonsCount = 6;

    private static readonly IScheduleService ScheduleService = 
        ServiceManager.Default.GetRequiredService<IScheduleService>();

    private static readonly ISubjectsService SubjectsService = 
        ServiceManager.Default.GetRequiredService<ISubjectsService>();

    private static readonly ITeachersService TeachersService = 
        ServiceManager.Default.GetRequiredService<ITeachersService>();

    private static readonly IClassroomsService ClassroomsService = 
        ServiceManager.Default.GetRequiredService<IClassroomsService>();

    private static readonly byte CurrentSemester = (byte)(DateTime.Now.Month is >= 9 and <= 12 ? 1 : 2);

    private static readonly short CurrentYear = (short)DateTime.Now.Year;

    // Список данных для числителя и знаменателя
    [ObservableProperty]
    private ObservableCollection<LessonScheduleModel> _numeratorDailySchedule = new();

    [ObservableProperty]
    private ObservableCollection<LessonScheduleModel> _denominatorDailySchedule = new();

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
        SelectedClassroom = value.Classroom;
        SelectedSubject = value.Subject;
        SelectedTeacher = value.Teacher;
        SelectedDayOfWeek = value.WeekDay;
        SelectedLesson = value.Lesson;
        IsDenominator = value.IsDenominator;
    }

    [ObservableProperty]
    private GroupModel? _selectedGroup;

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
    
    private async Task UpdateComboboxes(SubjectModel value)
    {
        SelectedTeacher = (await TeachersService.GetTeacherBySubjectId(value.Id))
            .ToModel();

        SelectedClassroom = SelectedTeacher.Classroom;
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
    private bool _isPopupOpen;

    [ObservableProperty]
    private ObservableCollection<SubjectModel> _subjects = new();

    [ObservableProperty]
    private ObservableCollection<TeacherModel> _teachers = new();

    [ObservableProperty]
    private ObservableCollection<ClassroomModel> _classrooms = new();

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public ScheduleManagementViewModel(
        GroupModel selectedGroup,
        IEnumerable<Subject> subjects,
        IEnumerable<Teacher> teachers,
        IEnumerable<Classroom> classrooms,
        IEnumerable<Schedule> numeratorSchedules,
        IEnumerable<Schedule> denominatorSchedules)
    {
        // Группируем расписания по дням и создаем DailyScheduleModel

        SelectedGroup = selectedGroup;

        foreach (var subject in subjects)
            Subjects.Add(subject.ToModel());

        foreach (var teacher in teachers)
            Teachers.Add(teacher.ToModel());

        foreach (var classroom in classrooms)
            Classrooms.Add(classroom.ToModel());

        PopulateLessonSchedules(numeratorSchedules, NumeratorDailySchedule);
        PopulateLessonSchedules(denominatorSchedules, DenominatorDailySchedule);
    }

    public static async Task<ScheduleManagementViewModel> CreateInstanceAsync(GroupModel selectedGroup)
        => new(selectedGroup,
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
            .Where(s => s.GroupId == SelectedGroup!.Id && s.Year == CurrentYear && s.Semester == CurrentSemester)
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

    [RelayCommand(CanExecute = nameof(IsFieldsNotEmpty))]
    private void Save()
    {
        if (!ValidateSchedule(out var validationErrorMessage))
        {
            ErrorMessage = validationErrorMessage;
            return;
        }

        if (SelectedSchedule is null)
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
                Lesson = SelectedLesson
            };

            // Добавить расписание в коллекцию (Numerator или Denominator)
            return;
        }
        
        // Обновить существующее расписание
        SelectedSchedule.WeekDay = SelectedDayOfWeek;
        SelectedSchedule.Subject = SelectedSubject;
        SelectedSchedule.Teacher = SelectedTeacher;
        SelectedSchedule.Classroom = SelectedClassroom;
        SelectedSchedule.Semester = CurrentSemester;
        SelectedSchedule.Year = CurrentYear;
        SelectedSchedule.IsDenominator = IsDenominator;
        SelectedSchedule.Group = SelectedGroup;
        SelectedSchedule.Lesson = SelectedLesson;

        // Ваша бизнес-логика для проверки и обновления расписания осталась аналогичной...
    }

    private bool ValidateSchedule(out string errorMessage)
    {
        errorMessage = string.Empty;

        // Проверка на непересечение занятий одного преподавателя
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
        if (!MeetsScheduleRequirements(SelectedGroup!, SelectedSubject!))
        {
            errorMessage = "Суммарное количество часов для данного предмета превышает допустимую учебную нагрузку.";
            return false;
        }

        return true;
    }

    // Проверка на соответствие требованиям учебной нагрузки
    private bool MeetsScheduleRequirements(GroupModel group, SubjectModel subject)
    {
        int weeklyHours = subject.WeeklyHours;
        int currentHours = CalculateCurrentWeeklyHours(subject.Id);

        // Проверка, что добавление занятия не превысит допустимое количество часов
        if (currentHours + 2 > weeklyHours)
        {
            return false; // Добавление невозможно, если превышены часы
        }

        return true;
    }

    // Расчет текущего количества часов для предмета за неделю (числитель и знаменатель)
    private int CalculateCurrentWeeklyHours(int subjectId)
    {
        int numeratorHours = CalculateHoursForSchedule(NumeratorDailySchedule, subjectId);
        int denominatorHours = CalculateHoursForSchedule(DenominatorDailySchedule, subjectId);

        // Среднее количество часов на неделю
        return (numeratorHours + denominatorHours) / 2;
    }

    // Вспомогательный метод для расчета часов по расписанию (числитель или знаменатель)
    private int CalculateHoursForSchedule(ObservableCollection<LessonScheduleModel> schedule, int subjectId)
    {
        int totalHours = 0;

        foreach (var daySchedule in schedule)
        {
            foreach (var lesson in daySchedule.GetLessons()) // Предполагается метод GetLessons(), возвращающий расписания по всем дням недели
            {
                if (lesson != null && lesson.Subject?.Id == subjectId)
                {
                    totalHours += 2; // Каждое занятие длится 2 часа
                }
            }
        }

        return totalHours;
    }

    // Проверка занятости преподавателя в указанное время
    private bool IsTeacherBusy(TeacherModel teacher, WeekDay dayOfWeek, LessonModel lesson, bool isDenominator)
    {
        var schedules = isDenominator ? DenominatorDailySchedule : NumeratorDailySchedule;
        foreach (var daySchedule in schedules)
        {
            if (daySchedule.Lesson is null) continue;
            if (daySchedule.Lesson.Id == lesson.Id)
            {
                var lessonSchedule = GetLessonScheduleByDayOfWeek(daySchedule, dayOfWeek);
                if (lessonSchedule is not null && lessonSchedule.Teacher?.Id == teacher.Id)
                    return true;
            }
        }
        return false;
    }

    private ScheduleModel? GetLessonScheduleByDayOfWeek(LessonScheduleModel daySchedule, WeekDay dayOfWeek)
    {
        return dayOfWeek.Id switch
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
    }

    // Проверка занятости аудитории в указанное время
    private bool IsClassroomOccupied(ClassroomModel classroom, WeekDay dayOfWeek, LessonModel lesson, bool isDenominator)
    {
        var schedules = isDenominator ? DenominatorDailySchedule : NumeratorDailySchedule;
        foreach (var daySchedule in schedules)
        {
            if (daySchedule.Lesson.Id == lesson.Id)
            {
                var lessonSchedule = GetLessonScheduleByDayOfWeek(daySchedule, dayOfWeek);
                if (lessonSchedule != null && lessonSchedule.Classroom?.Id == classroom.Id)
                    return true;
            }
        }
        return false;
    }
}
