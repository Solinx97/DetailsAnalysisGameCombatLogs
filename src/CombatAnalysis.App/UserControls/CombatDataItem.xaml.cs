using MahApps.Metro.IconPacks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CombatAnalysis.App.UserControls;

public partial class CombatDataItem : UserControl
{
    public CombatDataItem()
    {
        InitializeComponent();
    }

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(CombatDataItem));

    public PackIconMaterialKind Icon
    {
        get { return (PackIconMaterialKind)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(PackIconMaterialKind), typeof(CombatDataItem));

    public Brush Color1
    {
        get { return (Brush)GetValue(Color1Property); }
        set { SetValue(Color1Property, value); }
    }

    public static readonly DependencyProperty Color1Property =
        DependencyProperty.Register("Color1", typeof(Brush), typeof(CombatDataItem));
}
