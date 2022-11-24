using MvvmCross.Platforms.Wpf.Views;
using System.Windows;
using System.Windows.Controls;

namespace CombatAnalysis.App.Views;

public partial class LoginView : MvxWpfView
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext != null)
        { ((dynamic)DataContext).Password = ((PasswordBox)sender).Password; }
    }
}
