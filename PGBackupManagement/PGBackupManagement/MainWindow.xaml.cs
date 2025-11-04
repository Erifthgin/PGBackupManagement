using Microsoft.Win32;
using PGBackupManagement.Models;
using System.Windows;
using System.Windows.Controls;

namespace PGBackupManagement;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public SettingModel SettingModel { get; init; }

    public MainWindow()
    {
        SettingModel = new();
        InitializeComponent();
        DataContext = this;
    }

    private void ConnectionMatchCheck(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.IsChecked is true)
        {
            SettingModel.InternalServer.Copy(SettingModel.ExternalServer);
            OnStateChanged(e);
        }
    }

    private void SelectFolderClick(object sender, RoutedEventArgs e)
    {
        var folderDialog = new OpenFolderDialog();

        if (folderDialog.ShowDialog() is true)
        {
            FolderPath.Text = folderDialog.FolderName;
        }
    }

    private async void RunClick(object sender, RoutedEventArgs e)
    {
        ChangeButtonEnabled(true);

        try
        {
            await Task.Run(() => PGProcessor.RunBackup(SettingModel, ExternalServerPassword.Password, InternalServerPassword.Password));
        }
        finally
        {
            ChangeButtonEnabled(false);
        }
    }

    private void ChangeButtonEnabled(bool enabled)
    {
        LoadingProgressBar.IsActive = enabled;
        RunBackupButton.IsEnabled = !enabled;
    }
}