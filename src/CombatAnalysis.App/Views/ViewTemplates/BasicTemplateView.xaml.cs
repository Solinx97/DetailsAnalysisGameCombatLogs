using CombatAnalysis.WinCore;
using MvvmCross.Platforms.Wpf.Views;
using System.Windows;

namespace CombatAnalysis.App.Views.ViewTemplates;

public partial class BasicTemplateView : MvxWpfView
{
    public BasicTemplateView()
    {
        InitializeComponent();
    }

    private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
        {
            WindowManager.MainWindow.DragMove();
        }
    }

    private void Minimaze(object sender, RoutedEventArgs e)
    {
        WindowManager.MainWindow.WindowState = WindowState.Minimized;
    }

    private void Maximaze(object sender, RoutedEventArgs e)
    {
        WindowManager.MainWindow.WindowState = WindowManager.MainWindow.WindowState != WindowState.Maximized
            ? WindowState.Maximized : WindowState.Normal;
    }
}
