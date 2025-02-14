﻿using System.Windows;
using System.Windows.Controls;

namespace CombatAnalysis.App.UserControls;

public partial class SystemMessageChat : UserControl
{
    public SystemMessageChat()
    {
        InitializeComponent();
    }

    public string Message
    {
        get { return (string)GetValue(MessageProperty); }
        set { SetValue(MessageProperty, value); }
    }

    public static readonly DependencyProperty MessageProperty =
        DependencyProperty.Register("Message", typeof(string), typeof(SystemMessageChat));
}
