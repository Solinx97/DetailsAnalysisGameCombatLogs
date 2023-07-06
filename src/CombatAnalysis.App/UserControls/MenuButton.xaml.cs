using MahApps.Metro.IconPacks;
using MvvmCross.Commands;
using System.Windows;
using System.Windows.Controls;

namespace CombatAnalysis.App.UserControls;

public partial class MenuButton : UserControl
{
    public MenuButton()
    {
        InitializeComponent();
    }

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register("Title", typeof(string), typeof(MenuButton));

    public IMvxCommand OnClick
    {
        get { return (IMvxCommand)GetValue(OnClickProperty); }
        set { SetValue(OnClickProperty, value); }
    }

    public static readonly DependencyProperty OnClickProperty =
        DependencyProperty.Register("OnClick", typeof(IMvxCommand), typeof(MenuButton));

    public bool IsActive
    {
        get { return (bool)GetValue(IsActiveProperty); }
        set { SetValue(IsActiveProperty, value); }
    }

    public static readonly DependencyProperty IsActiveProperty =
        DependencyProperty.Register("IsActive", typeof(bool), typeof(MenuButton));

    public PackIconMaterialKind Icon
    {
        get { return (PackIconMaterialKind)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(PackIconMaterialKind), typeof(MenuButton));
}
