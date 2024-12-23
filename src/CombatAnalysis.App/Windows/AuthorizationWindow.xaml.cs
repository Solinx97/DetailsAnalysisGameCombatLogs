using CombatAnalysis.Core.ViewModels.User;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using System;

namespace CombatAnalysis.App.Windows;

public partial class AuthorizationWindow : MvxWindow
{
    private readonly AuthorizationViewModel? _viewModel;

    public AuthorizationWindow()
    {
        InitializeComponent();

        DataContext = Mvx.IoCProvider.IoCConstruct<AuthorizationViewModel>();
        _viewModel = (AuthorizationViewModel)DataContext;

        ContentRendered += ContextRenderedHandler;
    }

    private void ContextRenderedHandler(object? sender, EventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.CloseAuthorizationWindow += CloseAuthorizationWindowHandler;
        }
    }

    private void CloseAuthorizationWindowHandler()
    {
        WindowManager.ExtraWindow?.Close();
        Close();
    }

    private void CloseWindowHandler(object sender, System.Windows.RoutedEventArgs e)
    {
        WindowManager.ExtraWindow?.Close();
        Close();
    }
}
