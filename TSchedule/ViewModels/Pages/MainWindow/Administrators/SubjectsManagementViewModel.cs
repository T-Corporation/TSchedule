using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Modern.Controls;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using TSchedule.Managers;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Models;

namespace TSchedule.ViewModels.Pages.MainWindow.Administrators;

public partial class SubjectsManagementViewModel : ObservableObject
{
    private readonly ISubjectsService SubjectsService
        = ServiceManager.Default.GetRequiredService<ISubjectsService>();

    public Flyout AttachedFlyout { get; }

    public FrameworkElement Target { get; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    [NotifyCanExecuteChangedFor(nameof(ShowEditFlyoutCommand))]
    private SubjectModel? _subject;

    [ObservableProperty]
    private ObservableCollection<SubjectModel> _subjects = [];

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private string _subjectCode = string.Empty;

    [ObservableProperty]
    private string _subjectName = string.Empty;

    [ObservableProperty]
    private string _subjectWeeklyHours = "1";

    [ObservableProperty]
    private SpecialtyModel? _specialty;

    [ObservableProperty]
    private ObservableCollection<SpecialtyModel> _specialties = [];

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public SubjectsManagementViewModel(
        IEnumerable<Subject> subjects,
        IEnumerable<Specialty> specialties,
        FrameworkElement element,
        Flyout flyout)
    {
        Target = element;
        AttachedFlyout = flyout;

        foreach (var subject in subjects)
            Subjects.Add(subject.ToModel());

        foreach (var specialty in specialties)
            Specialties.Add(specialty.ToModel());
    }

    public static async Task<SubjectsManagementViewModel> CreateInstanceAsync(FrameworkElement element, Flyout flyout)
        => new SubjectsManagementViewModel(
            await ServiceManager.Default.GetRequiredService<ISubjectsService>().GetAllSubjects(),
            await ServiceManager.Default.GetRequiredService<ISpecialtiesService>().GetAllSpecialties(),
            element,
            flyout);

    [RelayCommand]
    private void ShowFlyout()
    {
        IsEditing = false;
        ErrorMessage = string.Empty;
        SubjectCode = string.Empty;
        SubjectName = string.Empty;
        Specialty = Specialties.First();
        SubjectWeeklyHours = "1";
        AttachedFlyout.ShowAt(Target);
    }

    private bool IsSubjectNotEmpty() => Subject is not null;

    [RelayCommand(CanExecute = nameof(IsSubjectNotEmpty))]
    private void ShowEditFlyout()
    {
        IsEditing = true;
        ErrorMessage = string.Empty;
        SubjectCode = Subject!.Code;
        SubjectName = Subject.Name;
        SubjectWeeklyHours = Subject.WeeklyHours.ToString();
        Specialty = Subject.Specialty;
        AttachedFlyout.ShowAt(Target);
    }

    [RelayCommand]
    private void HideFlyout() => AttachedFlyout.Hide();

    [RelayCommand]
    private async Task AddOrEdit()
    {
        ErrorMessage = string.Empty;
        const string pleaseFillField = "Пожалуйста, заполните поле \"{0}\"";

        if (string.IsNullOrEmpty(SubjectCode))
        {
            ErrorMessage = string.Format(pleaseFillField, "Код");
            return;
        }

        if (string.IsNullOrEmpty(SubjectName))
        {
            ErrorMessage = string.Format(pleaseFillField, "Название");
            return;
        }

        if (string.IsNullOrEmpty(SubjectWeeklyHours))
        {
            ErrorMessage = string.Format(pleaseFillField, "Часы в неделю");
            return;
        }

        if (Specialty is null)
        {
            ErrorMessage = string.Format(pleaseFillField, "Специальность");
            return;
        }

        Subject subject = new()
        {
            Code = SubjectCode,
            Name = SubjectName,
            SpecialtyId = Specialty.Id,
            WeeklyHours = byte.Parse(SubjectWeeklyHours)
        };

        if (IsEditing)
        {
            if (Subject is null)
            {
                WindowManager.ShowMessageBox(
                    text: "Не выбран предмет",
                    caption: "Ошибка",
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.Error);
                return;
            }

            try
            {
                subject.Id = Subject.Id;
                await SubjectsService.UpdateSubject(subject);
                var foundSubject = Subjects.First(s => s.Id == Subject.Id);
                foundSubject.Code = subject.Code;
                foundSubject.Name = subject.Name;
                foundSubject.WeeklyHours = subject.WeeklyHours;
                foundSubject.Specialty = Specialty;
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
            await SubjectsService.AddSubject(subject);
            subject.Specialty = Specialty.ToEntity();
            Subjects.Add(subject.ToModel());
            HideFlyout();
        }
        catch (UniqueException ue)
        {
            ErrorMessage = ue.Message;
        }
    }

    [RelayCommand(CanExecute = nameof(IsSubjectNotEmpty))]
    private async Task Delete()
    {
        if (Subject is null)
        {
            WindowManager.ShowMessageBox(
                text: "Не выбран предмет",
                caption: "Ошибка",
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error);
            return;
        }

        if (WindowManager.ShowMessageBox(
            text: "Вы уверены, что хотите удалить предмет?",
            caption: "Подтверждение",
            button: MessageBoxButton.YesNo,
            icon: MessageBoxImage.Question) is not MessageBoxResult.Yes)
            return;

        await SubjectsService.RemoveSubject(Subject.Id);
        Subjects.Remove(Subjects.First(s => s.Id == Subject.Id));
    }
}
