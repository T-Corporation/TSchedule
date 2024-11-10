using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TSchedule.Managers;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Models;

namespace TSchedule.ViewModels.Pages.MainWindow;

public partial class ScheduleViewModel : ObservableObject
{
    private const int TotalLessonsCount = 6;

    private static readonly IScheduleService ScheduleService = 
        ServiceManager.Default.GetRequiredService<IScheduleService>();

    // Список данных для числителя и знаменателя
    [ObservableProperty]
    private ObservableCollection<LessonScheduleModel> _numeratorDailySchedule = [];

    [ObservableProperty]
    private ObservableCollection<LessonScheduleModel> _denominatorDailySchedule = [];

    [ObservableProperty]
    private GroupModel? _selectedGroup;

    [ObservableProperty]
    private byte _selectedSemester;

    [ObservableProperty]
    private short _selectedYear;

    private ScheduleViewModel(
        GroupModel selectedGroup,
        byte selectedSemester,
        short selectedYear,
        IEnumerable<Schedule> numeratorSchedules,
        IEnumerable<Schedule> denominatorSchedules)
    {
        // Группируем расписания по дням и создаем DailyScheduleModel
        SelectedGroup = selectedGroup;
        SelectedSemester = selectedSemester;
        SelectedYear = selectedYear;

        PopulateLessonSchedules(numeratorSchedules, NumeratorDailySchedule);
        PopulateLessonSchedules(denominatorSchedules, DenominatorDailySchedule);
    }

    public static async Task<ScheduleViewModel> CreateInstanceAsync(
        GroupModel selectedGroup,
        byte selectedSemester,
        short selectedYear)
        => new(
            selectedGroup,
            selectedSemester,
            selectedYear,
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

        for (var lessonNumber = 1; lessonNumber <= TotalLessonsCount; lessonNumber++)
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

    [RelayCommand]
    private void OpenExportWizard()
    {
        if (SelectedGroup is null) return;

        WindowManager.Default.CreateWindowWithParameters<Views.ImportExportWindow>(
            showDialog: true,
            parameters:
            [
                WizardType.Export,
                new ExcelManager.LessonSchedules(SelectedGroup, SelectedSemester, SelectedYear, NumeratorDailySchedule, DenominatorDailySchedule)
            ]);
    }
}
