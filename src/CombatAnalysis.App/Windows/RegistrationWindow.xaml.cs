using CombatAnalysis.Core.ViewModels.User;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using System;

namespace CombatAnalysis.App.Windows;

public partial class RegistrationWindow : MvxWindow
{
    private readonly RegistrationViewModel? _viewModel;

    public RegistrationWindow()
    {
        InitializeComponent();

        DataContext = Mvx.IoCProvider.IoCConstruct<RegistrationViewModel>();
        _viewModel = (RegistrationViewModel)DataContext;

        ContentRendered += ContextRenderedHandler;
    }

    private void ContextRenderedHandler(object? sender, EventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.CloseRegistrationWindow += CloseRegistrationWindowHandler;
            _viewModel.SendRequest();
        }
    }

    private void CloseRegistrationWindowHandler()
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