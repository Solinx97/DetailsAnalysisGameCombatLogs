using CombatAnalysis.Core.ViewModels.User;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
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
        ((BasicTemplateViewModel)_viewModel.Basic).AuthorizationIsOpen = true;

        ContentRendered += ContextRenderedHandler;
    }

    private void ContextRenderedHandler(object? sender, EventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.CloseAuthorizationWindow += CloseAuthorizationWindowHandler;
            _viewModel.SendRequest();
        }
    }

    private void CloseAuthorizationWindowHandler()
    {
        WindowManager.ExtraWindow?.Close();
        Close();
    }

    private void CloseWindowHandler(object sender, System.Windows.RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            ((BasicTemplateViewModel)_viewModel.Basic).AuthorizationIsOpen = false;
        }

        WindowManager.ExtraWindow?.Close();
        Close();
    }
}
