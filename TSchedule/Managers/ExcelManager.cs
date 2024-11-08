using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using TSchedule.Persistence.Models;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Extensions;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Enums;
using TSchedule.Extensions;

namespace TSchedule.Managers;

public class ExcelManager
{
    public static readonly string AppDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
        "TSchedule");

    public static readonly ISubjectsService SubjectsService = 
        ServiceManager.Default.GetRequiredService<ISubjectsService>();

    public static readonly ITeachersService TeachersService = 
        ServiceManager.Default.GetRequiredService<ITeachersService>();

    public static readonly IGroupsService GroupsService =
        ServiceManager.Default.GetRequiredService<IGroupsService>();

    public string FilePath { get; protected set; }

    public ExcelVersion Version { get; protected set; }

    public bool FirstRowContainsHeaders { get; protected set; } = true;

    public ExcelManager(
        string filePath = "",
        ExcelVersion version = ExcelVersion.Excel97,
        bool firstRowContainsHeaders = true)
    {
        filePath = string.IsNullOrEmpty(filePath)
            ? Path.Combine(AppDirectory, GenerateUniqueName(version is ExcelVersion.Excel97
                ? "Расписание.xls"
                : "Расписание.xlsx"))
            : filePath;

        CheckVersionConflict(filePath, version);

        FilePath = filePath;
        Version = version;
        FirstRowContainsHeaders = firstRowContainsHeaders;
    }

    #region Public
    public ExcelManager SetFilePath(string filePath)
    {
        FilePath = filePath;
        return this;
    }

    public ExcelManager SetExcelVersion(ExcelVersion version)
    {
        Version = version;
        return this;
    }

    public ExcelManager SetFirstRowContainsHeaders(bool firstRowContainsHeaders)
    {
        FirstRowContainsHeaders = firstRowContainsHeaders;
        return this;
    }

    public static string ExcelVersionToFileFilter(ExcelVersion version) => version switch
    {
        ExcelVersion.Excel2007 => "Excel файлы (*.xlsx)|*.xlsx",
        _ => "Excel файлы (*.xls)|*.xls"
    };

    public record LessonSchedules(
        GroupModel Group,
        byte Semester,
        short Year,
        ICollection<LessonScheduleModel> NumeratorDailySchedule,
        ICollection<LessonScheduleModel> DenominatorDailySchedule)
    {
        public bool IsEmpty() => Group.Equals(GroupModel.Unknown)
            || Semester == 0
            || Year == 0
            || NumeratorDailySchedule.Count == 0
            || DenominatorDailySchedule.Count == 0;
    }

    public async Task<LessonSchedules> GetSchedulesFromFile()
    {
        CheckVersionConflict(FilePath, Version);

        ICollection<LessonScheduleModel> numeratorDailySchedule = [];
        ICollection<LessonScheduleModel> denominatorDailySchedule = [];

        if (!File.Exists(FilePath))
            throw new FileNotFoundException($"Файл по адресу \"{FilePath}\" не найден.");

        using var package = new ExcelPackage(new FileInfo(FilePath));
        var numeratorSheet = package.Workbook.Worksheets["Числитель"];
        var denominatorSheet = package.Workbook.Worksheets["Знаменатель"];

        if (numeratorSheet is null || denominatorSheet is null)
            throw new NotSupportedException(
                "Неверный формат файла^ Один или оба листа не найдены. Следуйте шаблону, чтобы не получать такие ошибки.");

        // Получаем код группы из первой строки первого листа
        var scheduleInfo = numeratorSheet.Cells["A1"].Text.Trim();
        var scheduleInfoParts = scheduleInfo.Split("\n");

        if (scheduleInfoParts.Length != 3)
            throw new InvalidDataException(
                $"""
                Неверный формат информации о расписании. Ожидалось:
                `
                Группа: $Название
                Семестр: $Семестр
                Год: $Год
                `
                А получено:
                `
                {scheduleInfo}
                `.
                """);

        var groupParts = scheduleInfoParts[0].Split(':');

        if (groupParts.Length != 2
            || groupParts[0].Trim() != "Группа")
            throw new InvalidDataException(
                $"Неверный формат группы. Ожидалось: \"Группа: $Название\", а получено: \"{scheduleInfoParts[0]}\".");

        var groupCode = groupParts[1].Trim();
        var group = await GroupsService.GetGroupByCode(groupCode)
            ?? throw new InvalidDataException($"Группа с кодом \"{scheduleInfo}\" не найдена.");

        var semesterParts = scheduleInfoParts[1].Split(':');

        if (semesterParts.Length != 2
            || semesterParts[0].Trim() != "Семестр"
            || !byte.TryParse(semesterParts[1], out var semester)
            || semester is < 1 or > 2)
            throw new InvalidDataException(
                $"Неверный формат семестра. Ожидалось: \"Семестр: (1 или 2)\", а получено: \"{scheduleInfoParts[1]}\"");

        var yearParts = scheduleInfoParts[2].Split(':');
        if (yearParts.Length != 2
            || !short.TryParse(yearParts[1], out var year))
            throw new InvalidDataException(
                $"Неверный формат года. Ожидалось: \"Год: 0000\", а получено: \"{scheduleInfoParts[2]}\"");

        // Параметр _startRow будет определять, какую строку начинать с
        var startRow = FirstRowContainsHeaders ? 3 : 2;

        await ParseSheets(numeratorSheet, numeratorDailySchedule, denominatorDailySchedule, group, isDenominator: false, startRow);
        await ParseSheets(denominatorSheet, numeratorDailySchedule, denominatorDailySchedule, group, isDenominator: true, startRow);

        return new LessonSchedules(group.ToModel(), semester, year, numeratorDailySchedule, denominatorDailySchedule);
    }

    public async Task ExportScheduleToFile(LessonSchedules schedules)
    {
        CheckVersionConflict(FilePath, Version);

        using ExcelPackage package = new();

        // Создаем листы для числителя и знаменателя
        var numeratorSheet = package.Workbook.Worksheets.Add("Числитель");
        var denominatorSheet = package.Workbook.Worksheets.Add("Знаменатель");

        // Устанавливаем название группы в первой строке
        numeratorSheet.Cells["A1"].Value = $"Группа: {schedules.Group.Code}\nСеместр: {schedules.Semester}\nГод: {schedules.Year}";
        denominatorSheet.Cells["A1"].Value = $"Группа: {schedules.Group.Code}\nСеместр: {schedules.Semester}\nГод: {schedules.Year}";

        // Объединяем ячейки для заголовка группы
        numeratorSheet.Cells["A1:H1"].Merge = true;
        numeratorSheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        numeratorSheet.Cells["A2:H2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
    
        denominatorSheet.Cells["A1:H1"].Merge = true;
        denominatorSheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        denominatorSheet.Cells["A2:H2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        // Добавляем заголовки с учётом сдвига на одну строку вниз
        AddHeaders(numeratorSheet, rowOffset: 2);
        AddHeaders(denominatorSheet, rowOffset: 2);

        // Экспортируем расписание числителя и знаменателя
        PopulateSheet(numeratorSheet, schedules.NumeratorDailySchedule, startRow: 3);
        PopulateSheet(denominatorSheet, schedules.DenominatorDailySchedule, startRow: 3);

        // Сохраняем файл с обработкой исключений
        try
        {
            await package.SaveAsAsync(new FileInfo(FilePath));
        }
        catch (Exception ex)
        {
            throw new IOException(
                $"Ошибка сохранения файла: убедитесь, что этот файл не открыт.", ex);
        }
    }
    #endregion

    #region Private
    private static void CheckVersionConflict(string filePath, ExcelVersion version)
    {
        var fileExtension = Path.GetExtension(filePath);

        if (fileExtension != version.GetExtension())
            throw new ArgumentException($"Выбрана неправильная версия файла Excel: получена {version}, а ожидалась: {(
                version is ExcelVersion.Excel97
                    ? ExcelVersion.Excel2007
                    : ExcelVersion.Excel97)}", nameof(version));
    }

    private static string GenerateUniqueName(string baseFileName)
    {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(baseFileName);
        string extension = Path.GetExtension(baseFileName);
        string uniqueFileName = baseFileName;

        int fileIndex = 1;

        // Проверяем, существует ли файл с текущим именем
        while (File.Exists(Path.Combine(AppDirectory, uniqueFileName)))
            uniqueFileName = $"{fileNameWithoutExtension}{fileIndex++}{extension}";

        return uniqueFileName;
    }

    private static async Task ParseSheets(
        ExcelWorksheet sheet,
        ICollection<LessonScheduleModel> numeratorDailySchedule,
        ICollection<LessonScheduleModel> denominatorDailySchedule,
        Group group,
        bool isDenominator,
        int startRow)
    {
        for (int row = startRow; row <= sheet.Dimension.End.Row; row++) // Обрабатываем все строки до конца
        {
            var lessonSchedule = new LessonScheduleModel { Lesson = Lessons.GetLessonById(row - startRow + 1)! }; // Сдвигаем Id урока

            for (int col = 2; col <= 8; col++) // Дни недели (Пн-Вс)
            {
                var scheduleInfo = sheet.Cells[row, col].Text.Trim();

                if (string.IsNullOrEmpty(scheduleInfo))
                    continue;

                var scheduleInfoParts = scheduleInfo.Split("\n");

                if (scheduleInfoParts.Length != 3)
                    throw new ArgumentException(
                        """
                        Неверный формат данных о занятии, должен соблюдаться следующий формат:
                        $НазваниеПредмета
                        $НомерКабинета
                        $ФиоПреподавателя
                        """,
                        string.Empty,
                        null);

                var subjectName = scheduleInfoParts[0].Trim();
                var subject = await SubjectsService.GetSubjectByName(subjectName)
                    ?? throw new InvalidDataException($"Предмет по названию \"{subjectName}\" не найден");

                var teacher = await TeachersService.GetTeacherBySubjectId(subject.Id)
                    ?? throw new InvalidDataException($"Предмет \"{subjectName}\" не ведёт ни один преподаватель");
            
                var schedule = new ScheduleModel
                {
                    Group = group.ToModel(),
                    WeekDay = WeekDays.GetWeekDayById(col - 1), // Используем номер дня (Пн-Вс)
                    Teacher = teacher.ToModel(),
                    Lesson = Lessons.GetLessonById(row - startRow + 1), // Текущий номер урока
                    IsDenominator = isDenominator
                };

                if (!ValidateSchedule(schedule, isDenominator, numeratorDailySchedule, denominatorDailySchedule, out var errorMessage))
                    throw new InvalidDataException(errorMessage);

                SetLessonScheduleByDay(lessonSchedule, schedule);
            }

            if (isDenominator)
                denominatorDailySchedule.Add(lessonSchedule);
            else
                numeratorDailySchedule.Add(lessonSchedule);
        }
    }

    private static bool ValidateSchedule(
        ScheduleModel schedule,
        bool isDenominator,
        ICollection<LessonScheduleModel> numeratorDailySchedule,
        ICollection<LessonScheduleModel> denominatorDailySchedule,
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
        ICollection<LessonScheduleModel> numeratorDailySchedule,
        ICollection<LessonScheduleModel> denominatorDailySchedule)
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
        ICollection<LessonScheduleModel> numeratorDailySchedule,
        ICollection<LessonScheduleModel> denominatorDailySchedule)
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
        ICollection<LessonScheduleModel> numeratorDailySchedule,
        ICollection<LessonScheduleModel> denominatorDailySchedule)
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
        ICollection<LessonScheduleModel> numeratorDailySchedule,
        ICollection<LessonScheduleModel> denominatorDailySchedule)
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

    // Метод для добавления заголовков в лист с учетом сдвига
    private static void AddHeaders(ExcelWorksheet sheet, int rowOffset)
    {
        sheet.DefaultRowHeight = 65;
        sheet.DefaultColWidth = 25;

        sheet.Cells[rowOffset, 1].Value = "Урок";
        sheet.Cells[rowOffset, 2].Value = "Понедельник";
        sheet.Cells[rowOffset, 3].Value = "Вторник";
        sheet.Cells[rowOffset, 4].Value = "Среда";
        sheet.Cells[rowOffset, 5].Value = "Четверг";
        sheet.Cells[rowOffset, 6].Value = "Пятница";
        sheet.Cells[rowOffset, 7].Value = "Суббота";
        sheet.Cells[rowOffset, 8].Value = "Воскресенье";

        // Применяем стиль для заголовков
        using var range = sheet.Cells[rowOffset, 1, rowOffset, 8];
        range.Style.Font.Bold = true;
        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

        // Устанавливаем ширину колонок
        sheet.Column(1).Width = 10; // Первая колонка

        // Выровнять заголовки по центру и поддержка переноса
        for (int col = 1; col <= 8; col++)
        {
            sheet.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells[1, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells[1, col].Style.WrapText = true; // Поддержка переноса строк
        }
    }

    // Метод для заполнения листа данными расписания
    private static void PopulateSheet(
        ExcelWorksheet sheet,
        ICollection<LessonScheduleModel> dailySchedule,
        int startRow)
    {
        for (int i = 0; i < dailySchedule.Count; i++)
        {
            var lessonSchedule = dailySchedule.ElementAt(i);
            sheet.Cells[startRow + i, 1].Value = lessonSchedule.Lesson?.Id;

            // Заполняем расписание для каждого дня недели
            sheet.Cells[startRow + i, 2].Value = ScheduleToCellInfo(lessonSchedule.Monday);
            sheet.Cells[startRow + i, 3].Value = ScheduleToCellInfo(lessonSchedule.Tuesday);
            sheet.Cells[startRow + i, 4].Value = ScheduleToCellInfo(lessonSchedule.Wednesday);
            sheet.Cells[startRow + i, 5].Value = ScheduleToCellInfo(lessonSchedule.Thursday);
            sheet.Cells[startRow + i, 6].Value = ScheduleToCellInfo(lessonSchedule.Friday);
            sheet.Cells[startRow + i, 7].Value = ScheduleToCellInfo(lessonSchedule.Saturday);
            sheet.Cells[startRow + i, 8].Value = ScheduleToCellInfo(lessonSchedule.Sunday);

            // Выравнивание и поддержка переноса текста в ячейках
            for (int col = 1; col <= 8; col++)
            {
                sheet.Cells[startRow + i, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[startRow + i, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[startRow + i, col].Style.WrapText = true; // Поддержка переноса строк
            }
        }

        // Добавление границ ко всем ячейкам с заполненными данными
        var endRow = startRow + dailySchedule.Count - 1;
        var rangeWithBorders = sheet.Cells[1, 1, endRow, 8];

        // Применяем границы ко всем ячейкам в области
        rangeWithBorders.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        rangeWithBorders.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        rangeWithBorders.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        rangeWithBorders.Style.Border.Right.Style = ExcelBorderStyle.Thin;
    }

    private static string ScheduleToCellInfo(ScheduleModel? schedule)
    {
        if (schedule is null
            || schedule.Teacher is null
            || schedule.Teacher.Subject is null
            || schedule.Teacher.Classroom is null)
            return string.Empty;

        var teacher = schedule.Teacher;
        var subject = teacher.Subject;
        var classroom = teacher.Classroom;

        return $"{subject.Name}\n{teacher.FullName.ToShortName()}\n{classroom.Number}";
    }
    #endregion
}
