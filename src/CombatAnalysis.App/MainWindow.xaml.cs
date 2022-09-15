using CombatAnalysis.App.Windows;
using MvvmCross.Platforms.Wpf.Views;
using System.Windows;

namespace CombatAnalysis.App
{
    public partial class MainWindow : MvxWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Core.WindowManager.MainWindow = this;
            Core.WindowManager.CreateGroupChat = new CreateGroupChatWindow();

            Application.Current.MainWindow.Height = SystemParameters.PrimaryScreenHeight * 0.925;
            Application.Current.MainWindow.Width = SystemParameters.PrimaryScreenWidth * 0.925;
        }
    }
}
