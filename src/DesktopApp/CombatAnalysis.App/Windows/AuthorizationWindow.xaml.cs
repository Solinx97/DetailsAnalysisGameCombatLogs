using CombatAnalysis.Core.ViewModels.User;
using MvvmCross.Platforms.Wpf.Views;

namespace CombatAnalysis.App.Windows;

public partial class AuthorizationWindow : MvxWindow
{
    public AuthorizationWindow()
    {
        InitializeComponent();

        Loaded += async (_, _) =>
        {
            if (DataContext is AuthorizationViewModel vm)
            {
                await vm.Initialize();
            }
        };
    }

    private void CloseWindowHandler(object sender, System.Windows.RoutedEventArgs e)
    {
        Close();
    }
}
