using CombatAnalysis.App.Windows;
using MvvmCross.Platforms.Wpf.Views;
using System.Windows;

namespace CombatAnalysis.App;

public partial class MainWindow : MvxWindow
{
    public MainWindow()
    {
        InitializeComponent();

        WindowManager.MainWindow = this;
        WindowManager.CreateGroupChat = new CreateGroupChatWindow();

        Application.Current.MainWindow.Height = SystemParameters.PrimaryScreenHeight * 0.925;
        Application.Current.MainWindow.Width = SystemParameters.PrimaryScreenWidth * 0.925;

        Closed += MainWindow_Closed;
    }

    private void MainWindow_Closed(object sender, System.EventArgs e)
    {
        Dispose();
    }
}
