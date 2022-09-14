using CombatAnalysis.Core;
using MvvmCross.Platforms.Wpf.Views;
using System.Windows;

namespace CombatAnalysis.App
{
    public partial class MainWindow : MvxWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Windows.MainWindow = this;

            Application.Current.MainWindow.Height = SystemParameters.PrimaryScreenHeight * 0.925;
            Application.Current.MainWindow.Width = SystemParameters.PrimaryScreenWidth * 0.925;
        }
    }
}
