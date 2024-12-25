using CombatAnalysis.Core.ViewModels.User;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using System;

namespace CombatAnalysis.App.Windows;

public partial class AuthorizationWindow : MvxWindow
{
    private readonly AuthorizationViewModel? _viewModel;
    private BasicTemplateViewModel? _basicViewModel;

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
            _basicViewModel = (BasicTemplateViewModel)_viewModel.Basic;
            _basicViewModel.AuthorizationIsOpen = true;

            _viewModel.CloseAuthorizationWindow += CloseAuthorizationWindowHandler;
        }
    }

    private void CloseAuthorizationWindowHandler()
    {
        if (_basicViewModel != null)
        {
            _basicViewModel.AuthorizationIsOpen = false;
        }

        WindowManager.ExtraWindow?.Close();
        Close();
    }

    private void CloseWindowHandler(object sender, System.Windows.RoutedEventArgs e)
    {
        if (_basicViewModel != null)
        {
            _basicViewModel.AuthorizationIsOpen = false;
        }

        WindowManager.ExtraWindow?.Close();
        Close();
    }
}
