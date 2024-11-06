using System.IO;
using OfficeOpenXml;
using System.Windows;
using TSchedule.Persistence.Models;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Extensions;
using TSchedule.Persistence.Entities;

namespace TSchedule.Managers;

public class ExcelManager
{
    private static readonly Lazy<ExcelManager> _instance = new(() => new ExcelManager());

    public static ExcelManager Default => _instance.Value;

    public static readonly ISubjectsService SubjectsService = 
        ServiceManager.Default.GetRequiredService<ISubjectsService>();

    public static readonly ITeachersService TeachersService = 
        ServiceManager.Default.GetRequiredService<ITeachersService>();

    public record LessonSchedules(
        List<LessonScheduleModel> NumeratorDailySchedule,
        List<LessonScheduleModel> DenominatorDailySchedule)
    {
        public bool IsEmpty() => NumeratorDailySchedule.Count == 0 || DenominatorDailySchedule.Count == 0;
    }

    public async Task<LessonSchedules> ParseSchedule(
        string filePath,
        GroupModel group)
    {
        List<LessonScheduleModel> numeratorDailySchedule = [];
        List<LessonScheduleModel> denominatorDailySchedule = [];

        if (!File.Exists(filePath))
        {
            WindowManager.ShowMessageBox("Файл не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            return new LessonSchedules([], []);
        }

        using var package = new ExcelPackage(new FileInfo(filePath));

        var numeratorSheet = package.Workbook.Worksheets["Числитель"];
        var denominatorSheet = package.Workbook.Worksheets["Знаменатель"];

        if (numeratorSheet is null || denominatorSheet is null)
        {
            WindowManager.ShowMessageBox(
                "Один или оба листа не найдены в файле.",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return new LessonSchedules([], []);
        }

        await ParseSheets(numeratorSheet, numeratorDailySchedule, denominatorDailySchedule, group, isDenominator: false);

        return new LessonSchedules(numeratorDailySchedule, denominatorDailySchedule);
    }

    private async Task ParseSheets(
        ExcelWorksheet sheet,
        List<LessonScheduleModel> numeratorDailySchedule,
        List<LessonScheduleModel> denominatorDailySchedule,
        GroupModel group,
        bool isDenominator)
    {
        for (int row = 2; row <= 7; row++) // Уроки 1-6
        {
            // Создаем новый объект расписания для текущего урока
            var lessonSchedule = new LessonScheduleModel { Lesson = Lessons.GetLessonById(row - 1)! }; // Уроки 1-6 => Id 1-6

            for (int col = 2; col <= 8; col++) // Дни недели (Пн-Вс)
            {
                string subjectName = sheet.Cells[row, col].Text.Trim();

                // Пропускаем, если ячейка пустая
                if (string.IsNullOrEmpty(subjectName))
                    continue;

                var subject = await SubjectsService.GetSubjectByName(subjectName); // Получаем модель предмета по имени

                if (subject is null)
                {
                    WindowManager.ShowMessageBox(
                        $"Предмет по названию \"{subjectName}\" не найден",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                var teacher = await TeachersService.GetTeacherBySubjectId(subject.Id); // Получаем преподавателя по предмету

                if (teacher is null)
                {
                    WindowManager.ShowMessageBox(
                        $"Предмет по названию \"{subjectName}\" не ведёт ни один преподаватель",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
                
                // Заполняем расписание по дням недели
                var schedule = new ScheduleModel
                {
                    Group = group,
                    WeekDay = WeekDays.GetWeekDayById(col - 1), // Используем номер дня (Пн-Вс)
                    Teacher = teacher.ToModel(),
                    Lesson = Lessons.GetLessonById(row - 1), // Текущий номер урока
                    IsDenominator = isDenominator
                };

                // Проверяем, есть ли конфликты
                if (!ValidateSchedule(schedule, isDenominator, numeratorDailySchedule, denominatorDailySchedule, out var errorMessage))
                {
                    WindowManager.ShowMessageBox(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Устанавливаем соответствующее поле в lessonSchedule
                SetLessonScheduleByDay(lessonSchedule, schedule);
            }

            // Добавляем заполненное расписание для текущего урока в список расписаний
            if (isDenominator)
                denominatorDailySchedule.Add(lessonSchedule);
            else
                numeratorDailySchedule.Add(lessonSchedule);
        }
    }

    private bool ValidateSchedule(
        ScheduleModel schedule,
        bool isDenominator,
        List<LessonScheduleModel> numeratorDailySchedule,
        List<LessonScheduleModel> denominatorDailySchedule,
        out string errorMessage)
    {
        errorMessage = string.Empty;

        // Проверка 1: Преподаватель занят
        if (IsTeacherBusy(schedule.Teacher!, schedule.WeekDay!, schedule.Lesson!, isDenominator, numeratorDailySchedule, denominatorDailySchedule))
        {
            errorMessage = "Преподаватель уже занят в указанное время.";
            return false;
        }

        // Проверка 2: Аудитория занята
        if (IsClassroomOccupied(schedule.Teacher!.Classroom!, schedule.WeekDay!, schedule.Lesson!, isDenominator, numeratorDailySchedule, denominatorDailySchedule))
        {
            errorMessage = "Аудитория уже занята в указанное время.";
            return false;
        }

        // Проверка 3: Превышение учебной нагрузки
        if (!MeetsScheduleRequirements(schedule.Teacher.Subject!, numeratorDailySchedule, denominatorDailySchedule))
        {
            errorMessage = "Количество часов превышает допустимую учебную нагрузку.";
            return false;
        }

        return true;
    }

    // Метод для установки расписания по дню недели
    private static void SetLessonScheduleByDay(LessonScheduleModel lessonSchedule, ScheduleModel schedule)
    {
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
    /// Проверяет, не превышает ли суммарное количество часов по предмету допустимую учебную нагрузку.
    /// </summary>
    /// <param name="subject">Предмет, для которого проверяется нагрузка.</param>
    /// <returns>Истина, если нагрузка не превышена, иначе ложь.</returns>
    private static bool MeetsScheduleRequirements(
        SubjectModel subject,
        List<LessonScheduleModel> numeratorDailySchedule,
        List<LessonScheduleModel> denominatorDailySchedule)
    {
        int weeklyHours = subject.WeeklyHours;
        int currentHours = CalculateCurrentWeeklyHours(subject.Id, numeratorDailySchedule, denominatorDailySchedule);

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
    private static int CalculateCurrentWeeklyHours(
        int subjectId,
        List<LessonScheduleModel> numeratorDailySchedule,
        List<LessonScheduleModel> denominatorDailySchedule)
    {
        int numeratorHours = CalculateHoursForSchedule(numeratorDailySchedule, subjectId);
        int denominatorHours = CalculateHoursForSchedule(denominatorDailySchedule, subjectId);

        // Среднее количество часов на неделю
        return (numeratorHours + denominatorHours) / 2;
    }

    /// <summary>
    /// Вычисляет общее количество часов по расписанию для указанного предмета.
    /// </summary>
    /// <param name="schedule">Коллекция расписаний.</param>
    /// <param name="subjectId">Идентификатор предмета.</param>
    /// <returns>Общее количество часов для указанного предмета.</returns>
    private static int CalculateHoursForSchedule(ICollection<LessonScheduleModel> schedule, int subjectId)
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
    private static bool IsTeacherBusy(
        TeacherModel teacher,
        WeekDay dayOfWeek,
        LessonModel lesson,
        bool isDenominator,
        List<LessonScheduleModel> numeratorDailySchedule,
        List<LessonScheduleModel> denominatorDailySchedule)
    {
        var schedules = isDenominator ? denominatorDailySchedule : numeratorDailySchedule;
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
    private static bool IsClassroomOccupied(
        ClassroomModel classroom,
        WeekDay dayOfWeek,
        LessonModel lesson,
        bool isDenominator,
        List<LessonScheduleModel> numeratorDailySchedule,
        List<LessonScheduleModel> denominatorDailySchedule)
    {
        var schedules = isDenominator ? denominatorDailySchedule : numeratorDailySchedule;
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
}

