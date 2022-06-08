using CombatAnalysis.Core;
using MvvmCross.Platforms.Wpf.Views;

namespace CombatAnalysis.App
{
    public partial class MainWindow : MvxWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            WindowCloser.MainWindow = this;
        }
    }
}
