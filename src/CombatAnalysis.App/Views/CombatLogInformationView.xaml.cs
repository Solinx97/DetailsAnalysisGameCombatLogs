using CombatAnalysis.Core.ViewModels;
using Microsoft.Win32;
using MvvmCross.Platforms.Wpf.Views;
using System;

namespace CombatAnalysis.App.Views;

public partial class CombatLogInformationView : MvxWpfView
{
    public CombatLogInformationView()
    {
        InitializeComponent();
    }

    private void SelectCmbatLogFile(object sender, System.Windows.RoutedEventArgs e)
    {
        var fileDialog = new OpenFileDialog
        {
            InitialDirectory = AppDomain.CurrentDomain.BaseDirectory
        };
        fileDialog.ShowDialog();

        ((CombatLogInformationViewModel)ViewModel).CombatLogPath = fileDialog.FileName;
    }
}
