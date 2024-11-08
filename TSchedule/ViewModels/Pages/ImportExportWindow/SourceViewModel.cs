using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using TSchedule.Managers;
using TSchedule.Persistence.Enums;

namespace TSchedule.ViewModels.Pages.ImportExportWindow;

public partial class SourceViewModel(WizardType wizardType) : ObservableObject
{
    [ObservableProperty]
    private string _filePath = string.Empty;

    [ObservableProperty]
    private ExcelVersion _selectedVersion = ExcelVersion.Excel97;

    [ObservableProperty]
    private ExcelVersion[] _versions = (ExcelVersion[])Enum.GetValues(typeof(ExcelVersion));

    [ObservableProperty]
    private bool _firstRowContainsHeaders = true;

    [ObservableProperty]
    private bool _isFirstRowContainsHeadersVisible = true;

    [RelayCommand]
    private void OpenDialog()
    {
        FileDialog fileDialog = wizardType is WizardType.Export
            ? new SaveFileDialog()
            : new OpenFileDialog();

        fileDialog.Filter = ExcelManager.ExcelVersionToFileFilter(SelectedVersion);
        if (fileDialog.ShowDialog() is null) return;
        FilePath = fileDialog.FileName;
    }
}
