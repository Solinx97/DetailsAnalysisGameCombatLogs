using CombatAnalysis.Core;
using MvvmCross.Platforms.Wpf.Views;

namespace CombatAnalysis.App.Views
{
    public partial class BasicTemplateView : MvxWpfView
    {
        public BasicTemplateView()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                Windows.MainWindow.DragMove();
            }
        }
    }
}
