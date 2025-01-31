using CombatAnalysis.App.Windows;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using MvvmCross.Platforms.Wpf.Views;
using System.Windows;

namespace CombatAnalysis.App.Views.ViewTemplates;

public partial class BasicTemplateView : MvxWpfView
{
    public BasicTemplateView()
    {
        InitializeComponent();

        DataContextChanged += BasicTemplateViewDataContextChanged;
    }

    private void BasicTemplateViewDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        var viewModel = (BasicTemplateViewModel)e.NewValue;
        if (viewModel != null)
        {
            viewModel.ClearEvents();
            viewModel.OpenAuthorizationWindow += OpenAuthorizationWindow;
            viewModel.OpenRegistrationWindow += OpenRegistrationWindow;
            viewModel.CloseAuthorizationWindow += CloseExtraWindow;
            viewModel.CloseRegistrationWindow += CloseExtraWindow;
        }
    }

    private void OpenAuthorizationWindow()
    {
        WindowManager.ExtraWindow = new AuthorizationWindow();
        WindowManager.ExtraWindow.Show();
    }

    private void OpenRegistrationWindow()
    {
        WindowManager.ExtraWindow = new RegistrationWindow();
        WindowManager.ExtraWindow.Show();
    }

    private void CloseExtraWindow()
    {
        WindowManager.ExtraWindow?.Close();
    }

    private void BorderMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
        {
            WindowManager.MainWindow?.DragMove();
        }
    }

    private void Minimaze(object sender, RoutedEventArgs e)
    {
        if (WindowManager.MainWindow != null)
        {
            WindowManager.MainWindow.WindowState = WindowState.Minimized;
        }
    }

    private void Maximaze(object sender, RoutedEventArgs e)
    {
        if (WindowManager.MainWindow != null)
        {
            WindowManager.MainWindow.WindowState = WindowManager.MainWindow.WindowState != WindowState.Maximized
                ? WindowState.Maximized : WindowState.Normal;
        }
    }
}
