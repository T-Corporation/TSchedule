using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using iNKORE.UI.WPF.Modern.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using TSchedule.Managers;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Exceptions;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Models;

namespace TSchedule.ViewModels.Pages.MainWindow.Administrators;

public partial class GroupsManagementViewModel : ObservableObject
{
    public readonly Lazy<IGroupsService> GroupsService
        = new(ServiceManager.Default.GetRequiredService<IGroupsService>);

    public Flyout AttachedFlyout { get; }

    public FrameworkElement Target { get; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    [NotifyCanExecuteChangedFor(nameof(ShowEditFlyoutCommand))]
    private GroupModel? _group;

    [ObservableProperty]
    private ObservableCollection<GroupModel> _groups = [];

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private string _groupCode = string.Empty;

    [ObservableProperty]
    private string _groupCourse = "1";

    [ObservableProperty]
    private SpecialtyModel? _specialty;

    [ObservableProperty]
    private ObservableCollection<SpecialtyModel> _specialties = [];

    [ObservableProperty]
    private ObservableCollection<SubjectModel> _subjects = [];

    [ObservableProperty]
    private ObservableCollection<SubjectModel> _selectedSubjects = [];

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public GroupsManagementViewModel(
        IEnumerable<Group> groups,
        IEnumerable<Specialty> specialties,
        IEnumerable<Subject> subjects,
        FrameworkElement element,
        Flyout flyout)
    {
        Target = element;
        AttachedFlyout = flyout;

        foreach (var group in groups)
            Groups.Add(group.ToModel());

        foreach (var specialty in specialties)
            Specialties.Add(specialty.ToModel());

        foreach (var subject in subjects)
            Subjects.Add(subject.ToModel());
    }

    public static async Task<GroupsManagementViewModel> CreateInstanceAsync(FrameworkElement element, Flyout flyout)
        => new GroupsManagementViewModel(
            await ServiceManager.Default.GetRequiredService<IGroupsService>().GetAllGroups(),
            await ServiceManager.Default.GetRequiredService<ISpecialtiesService>().GetAllSpecialties(),
            await ServiceManager.Default.GetRequiredService<ISubjectsService>().GetAllSubjects(),
            element,
            flyout);

    [RelayCommand]
    private void ShowFlyout()
    {
        IsEditing = false;
        ErrorMessage = string.Empty;
        GroupCode = string.Empty;
        GroupCourse = "1";
        Specialty = null;
        SelectedSubjects.Clear();
        AttachedFlyout.ShowAt(Target);
    }

    private bool IsGroupNotEmpty() => Group is not null;

    [RelayCommand(CanExecute = nameof(IsGroupNotEmpty))]
    private void ShowEditFlyout()
    {
        IsEditing = true;
        ErrorMessage = string.Empty;
        GroupCode = Group!.Code;
        GroupCourse = Group.Course.ToString();
        Specialty = Group.Specialty;

        // Выбор предметов, связанных с группой
        SelectedSubjects.Clear();
        foreach (var subject in Group.Subjects)
            SelectedSubjects.Add(new SubjectModel { SelectedSubject = subject });

        AttachedFlyout.ShowAt(Target);
    }

    [RelayCommand(CanExecute = nameof(IsGroupNotEmpty))]
    private async Task Delete()
    {
        if (WindowManager.ShowMessageBox(
            text: "Вы уверены, что хотите удалить группу?",
            caption: "Подтверждение",
            button: MessageBoxButton.YesNo,
            icon: MessageBoxImage.Question) is not MessageBoxResult.Yes)
            return;

        await GroupsService.Value.RemoveGroupAndSubjects(Group!.Id);
        Groups.Remove(Groups.First(g => g.Id == Group.Id));
    }

    [RelayCommand]
    private void HideFlyout() => AttachedFlyout.Hide();

    private bool IsSelectedSubjectsCountLessThanThousand() => SelectedSubjects.Count < 1000;

    // Команды для добавления, изменения и удаления предметов
    [RelayCommand(CanExecute = nameof(IsSelectedSubjectsCountLessThanThousand))]
    private void AddSubject()
        => SelectedSubjects.Add(new SubjectModel());

    [RelayCommand]
    private void DeleteSubject(SubjectModel subject)
        => SelectedSubjects.Remove(subject);

    // Обновленная команда для сохранения группы и её предметов
    [RelayCommand]
    private async Task AddOrEdit()
    {
        ErrorMessage = string.Empty;
        const string pleaseFillField = "Пожалуйста, заполните поле \"{0}\"";

        if (string.IsNullOrEmpty(GroupCode))
        {
            ErrorMessage = string.Format(pleaseFillField, "Код");
            return;
        }

        if (Specialty is null)
        {
            ErrorMessage = string.Format(pleaseFillField, "Специальность");
            return;
        }

        Group group = new()
        {
            Code = GroupCode,
            SpecialtyId = Specialty.Id,
            Course = byte.Parse(GroupCourse)
        };

        ObservableCollection<SubjectModel> trueSelectedSubjects = [];

        foreach (var subject in SelectedSubjects)
            if (subject.SelectedSubject is not null && !trueSelectedSubjects.Any(s => s.Id == subject.SelectedSubject.Id))
                trueSelectedSubjects.Add(subject.SelectedSubject);

        if (IsEditing)
        {
            if (Group is null)
            {
                WindowManager.ShowMessageBox(
                    text: "Не выбрана группа",
                    caption: "Ошибка",
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.Error);
                return;
            }

            try
            {
                group.Id = Group.Id;
                await GroupsService.Value.UpdateGroupAndSubjects(group, trueSelectedSubjects.Select(s => s.ToEntity()));
                var foundGroup = Groups.First(g => g.Id == Group.Id);
                foundGroup.Code = group.Code;
                foundGroup.Course = group.Course;
                foundGroup.Specialty = Specialty;
                foundGroup.Subjects = new ObservableCollection<SubjectModel>(trueSelectedSubjects);
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
            await GroupsService.Value.AddGroupAndSubjects(group, trueSelectedSubjects.Select(s => s.ToEntity()));
            var groupModel = group.ToModel();
            groupModel.Specialty = Specialty;
            groupModel.Subjects = new ObservableCollection<SubjectModel>(trueSelectedSubjects);
            Groups.Add(groupModel);
            HideFlyout();
        }
        catch (UniqueException ue)
        {
            ErrorMessage = ue.Message;
        }
    }
}
