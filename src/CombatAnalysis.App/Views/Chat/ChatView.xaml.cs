﻿using CombatAnalysis.App.Windows;
using MvvmCross.Platforms.Wpf.Views;

namespace CombatAnalysis.App.Views.Chat;

public partial class ChatView : MvxWpfView
{
    public ChatView()
    {
        InitializeComponent();
    }

    private void CreateGroupChat(object sender, System.Windows.RoutedEventArgs e)
    {
        WindowManager.ExtraWindow = new CreateGroupChatWindow();
        WindowManager.ExtraWindow?.Show();
    }
}
