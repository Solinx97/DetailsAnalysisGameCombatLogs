using CombatAnalysis.Core.ViewModels.CreateGroupChat;
using MvvmCross.Platforms.Wpf.Views;

namespace CombatAnalysis.App.Windows
{
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
            Core.WindowManager.CreateGroupChat = new CreateGroupChatWindow();
        }
    }
}
