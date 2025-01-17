using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.ViewModels.Chat;
using MvvmCross.Platforms.Wpf.Views;
using System.Threading.Tasks;
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
            Task.Run(async () => await groupChatMessagesVM.SendMessageHasBeenReadAsync(groupChatMessage));
        }
    }
}
