using CombatAnalysis.Core.ViewModels.Chat;
using MvvmCross.Commands;
using MvvmCross.Platforms.Wpf.Views;

namespace CombatAnalysis.App.Windows;

public partial class CreateGroupChatWindow : MvxWindow
{
    public CreateGroupChatWindow()
    {
        InitializeComponent();

        var createGroupChat = new CreateGroupChatViewModel
        {
            CancelCommand = new MvxCommand(() => WindowManager.CreateGroupChat.Close())
        };

        DataContext = createGroupChat;
        Closed += CreateGroupChatWindowClosed;
    }

    private void CreateGroupChatWindowClosed(object sender, System.EventArgs e)
    {
        WindowManager.CreateGroupChat = new CreateGroupChatWindow();
    }
}
