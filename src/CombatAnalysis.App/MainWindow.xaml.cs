using CombatAnalysis.App.Windows;
using MvvmCross.Platforms.Wpf.Views;
using System;
using System.Windows;

namespace CombatAnalysis.App;

public partial class MainWindow : MvxWindow
{
    public MainWindow()
    {
        Initialized += InitializedHandler;

        InitializeComponent();

        WindowManager.MainWindow = this;

        Application.Current.MainWindow.Height = SystemParameters.PrimaryScreenHeight * 0.925;
        Application.Current.MainWindow.Width = SystemParameters.PrimaryScreenWidth * 0.925;

        PreviewMouseDown += ShowAuthorizationWindow;
        Closed += MainWindowClosed;
    }

    private void InitializedHandler(object? sender, EventArgs e)
    {
        WindowManager.ExtraWindow = new AuthorizationWindow();
        WindowManager.ExtraWindow.Show();
    }

    private void ShowAuthorizationWindow(object? sender, EventArgs e)
    {
        if (WindowManager.ExtraWindow != null && WindowManager.ExtraWindow is AuthorizationWindow)
        {
            WindowManager.ExtraWindow.Focus();
        }
    }

    private void MainWindowClosed(object? sender, EventArgs e)
    {
        Dispose();
    }
}
