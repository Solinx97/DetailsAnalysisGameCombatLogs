using CombatAnalysis.Core.ViewModels.Chat;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using System.Windows;

namespace CombatAnalysis.App.Windows;

public partial class CreateGroupChatWindow : MvxWindow
{
    public CreateGroupChatWindow()
    {
        InitializeComponent();

        DataContextChanged += CreateGroupChatViewDataContextChanged;

        DataContext = Mvx.IoCProvider.IoCConstruct<CreateGroupChatViewModel>();
    }

    private void CreateGroupChatViewDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        var viewModel = (CreateGroupChatViewModel)e.NewValue;
        if (viewModel != null)
        {
            viewModel.ClearEvents();
            viewModel.CloseCreateChatWindow += CloseCreateChatWindowHandler;
        }
    }

    private void CloseCreateChatWindowHandler()
    {
        Close();
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
