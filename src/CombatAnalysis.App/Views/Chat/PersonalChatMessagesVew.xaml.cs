using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.ViewModels.Chat;
using MvvmCross.Platforms.Wpf.Views;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CombatAnalysis.App.Views.Chat;

public partial class PersonalChatMessagesVew : MvxWpfView
{
    public PersonalChatMessagesVew()
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

        var personalChatMessage = item.DataContext as PersonalChatMessageModel;
        if (personalChatMessage == null || personalChatMessage.Status == 2)
        {
            return;
        }

        var personalChatMessagesVM = DataContext as PersonalChatMessagesVewModel;
        if (personalChatMessagesVM != null)
        {
            Task.Run(async () => await personalChatMessagesVM.SendMessageHasBeenReadAsync(personalChatMessage));
        }
    }
}