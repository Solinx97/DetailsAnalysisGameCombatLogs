using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.ViewModels.Chat;
using MvvmCross.Platforms.Wpf.Views;
using System.Windows.Controls;

namespace CombatAnalysis.App.Views.Chat;

public partial class GroupChatMessagesView : MvxWpfView
{
    public GroupChatMessagesView()
    {
        InitializeComponent();
    }

    private void ListBoxItemMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        var item = sender as ListBoxItem;
        if (item == null)
        {
            return;
        }

        var groupChatMessage = item.DataContext as GroupChatMessageModel;
        if (groupChatMessage == null || groupChatMessage.Status == 2)
        {
            return;
        }

        var groupChatMessagesVM = DataContext as GroupChatMessagesViewModel;
        if (groupChatMessagesVM != null)
        {
            groupChatMessagesVM.MessageHasBeenReadCommand.Execute(groupChatMessage);
        }
    }

    private void SendMessageKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        var item = sender as TextBox;
        if (item == null)
        {
            return;
        }

        if (e.Key == System.Windows.Input.Key.Enter)
        {
            var groupChatMessagesVM = DataContext as GroupChatMessagesViewModel;
            if (groupChatMessagesVM != null)
            {
                groupChatMessagesVM.SendMessageKeyDownCommand.Execute(item.Text);
            }
        }
    }
}
