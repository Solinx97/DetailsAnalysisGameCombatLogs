using CombatAnalysis.Core.ViewModels.User;
using MvvmCross.Platforms.Wpf.Views;

namespace CombatAnalysis.App.Windows;

public partial class RegistrationWindow : MvxWindow
{
    public RegistrationWindow()
    {
        InitializeComponent();

        Loaded += async (_, _) =>
        {
            if (DataContext is RegistrationViewModel vm)
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