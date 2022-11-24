using MvvmCross.Platforms.Wpf.Views;
using System.Windows.Controls;

namespace CombatAnalysis.App.Views;

public partial class RegistrationView : MvxWpfView
{
    public RegistrationView()
    {
        InitializeComponent();
    }

    private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext != null)
        { ((dynamic)DataContext).Password = ((PasswordBox)sender).Password; }
    }

    private void ConfirmPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext != null)
        { ((dynamic)DataContext).ConfirmPassword = ((PasswordBox)sender).Password; }
    }
}
