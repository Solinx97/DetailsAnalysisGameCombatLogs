using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CombatAnalysis.App.UserControls;

public partial class MyMessageChat : UserControl
{
    public MyMessageChat()
    {
        InitializeComponent();
    }

    public string Message
    {
        get { return (string)GetValue(MessageProperty); }
        set { SetValue(MessageProperty, value); }
    }

    public static readonly DependencyProperty MessageProperty =
        DependencyProperty.Register("Message", typeof(string), typeof(MyMessageChat));

    public Brush Color
    {
        get { return (Brush)GetValue(ColorProperty); }
        set { SetValue(ColorProperty, value); }
    }

    public static readonly DependencyProperty ColorProperty =
        DependencyProperty.Register("Color", typeof(Brush), typeof(MyMessageChat));
}
