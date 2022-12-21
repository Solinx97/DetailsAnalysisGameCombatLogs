using MahApps.Metro.IconPacks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CombatAnalysis.App.UserControls;

public partial class PlayerCombatDataItem : UserControl
{
    public PlayerCombatDataItem()
    {
        InitializeComponent();
    }

    public PackIconMaterialKind Icon
    {
        get { return (PackIconMaterialKind)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(PackIconMaterialKind), typeof(PlayerCombatDataItem));

    public Brush GradientColor
    {
        get { return (Brush)GetValue(GradientColorProperty); }
        set { SetValue(GradientColorProperty, value); }
    }

    public static readonly DependencyProperty GradientColorProperty =
        DependencyProperty.Register("GradientColor", typeof(Brush), typeof(PlayerCombatDataItem));

    public Brush ProgressGradientColor
    {
        get { return (Brush)GetValue(ProgressGradientColorProperty); }
        set { SetValue(ProgressGradientColorProperty, value); }
    }

    public static readonly DependencyProperty ProgressGradientColorProperty =
        DependencyProperty.Register("ProgressGradientColor", typeof(Brush), typeof(PlayerCombatDataItem));

    public long Value
    {
        get { return (long)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(long), typeof(PlayerCombatDataItem));

    public long MaxValue
    {
        get { return (long)GetValue(MaxValueProperty); }
        set { SetValue(MaxValueProperty, value); }
    }

    public static readonly DependencyProperty MaxValueProperty =
        DependencyProperty.Register("MaxValue", typeof(long), typeof(PlayerCombatDataItem));
}
