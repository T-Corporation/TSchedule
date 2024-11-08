using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using TSchedule.Managers;
using TSchedule.Persistence.Enums;
using TSchedule.Persistence.Models;

namespace TSchedule.ViewModels.Pages.ImportExportWindow;

public partial class SourceViewModel(
    GroupModel group,
    byte semester,
    short year,
    WizardType wizardType) : ObservableObject
{
    [ObservableProperty]
    private string _filePath = string.Empty;

    [ObservableProperty]
    private ExcelVersion _selectedVersion = ExcelVersion.Excel97;

    [ObservableProperty]
    private ExcelVersion[] _versions = (ExcelVersion[])Enum.GetValues(typeof(ExcelVersion));

    [RelayCommand]
    private void OpenDialog()
    {
        FileDialog fileDialog = wizardType is WizardType.Export
            ? new SaveFileDialog()
            : new OpenFileDialog();
        fileDialog.FileName = $"Расписание_{year}_{semester}_{group.Code}.{SelectedVersion switch
        {
            ExcelVersion.Excel2007 => "xlsx",
            _ => "xls"
        }}";
        fileDialog.Filter = ExcelManager.ExcelVersionToFileFilter(SelectedVersion);
        if (fileDialog.ShowDialog() is null) return;
        FilePath = fileDialog.FileName;
    }
}
