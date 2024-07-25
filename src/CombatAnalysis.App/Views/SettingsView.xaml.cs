using CombatAnalysis.Core.ViewModels;
using CombatAnalysis.Core.ViewModels.Settings;
using MvvmCross.Platforms.Wpf.Views;
using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace CombatAnalysis.App.Views;

public partial class SettingsView : MvxWpfView
{
    public SettingsView()
    {
        InitializeComponent();
    }

    private void SelectLocation(object sender, System.Windows.RoutedEventArgs e)
    {
        var viewModel = (SettingsViewModel)ViewModel;

        var folderDialog = new FolderBrowserDialog
        {
            InitialDirectory = viewModel.LogsLocation
        };
        folderDialog.ShowDialog();

        if (string.IsNullOrEmpty(folderDialog.SelectedPath))
        {
            return;
        }

        viewModel.LogsLocation = folderDialog.SelectedPath;
        WriteUserSettings(folderDialog.SelectedPath, "user.json");
    }

    private static void WriteUserSettings(string locationPath, string settingsName)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        using var fs = new FileStream($"{baseDirectory}{settingsName}", FileMode.Create);
        var userSettings = new UserSettings(locationPath);

        JsonSerializer.Serialize(fs, userSettings);
    }
}
