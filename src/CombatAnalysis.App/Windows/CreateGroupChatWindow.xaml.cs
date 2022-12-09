using CombatAnalysis.Core.ViewModels.Chat;
using CombatAnalysis.WinCore;
using MvvmCross.Platforms.Wpf.Views;

namespace CombatAnalysis.App.Windows;

public partial class CreateGroupChatWindow : MvxWindow
{
    public CreateGroupChatWindow()
    {
        InitializeComponent();

        DataContext = new CreateGroupChatViewModel();

        Closed += CreateGroupChatWindowClosed;
    }

    private void CreateGroupChatWindowClosed(object sender, System.EventArgs e)
    {
        WindowManager.CreateGroupChat = new CreateGroupChatWindow();
    }
}
