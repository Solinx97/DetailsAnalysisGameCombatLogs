﻿namespace CombatAnalysis.WebApp.Models.Chat;

public class GroupChatMessageModel
{
    public int Id { get; set; }

    public string Message { get; set; }

    public string When { get; set; }

    public int Status { get; set; }

    public int Type { get; set; }

    public int GroupChatId { get; set; }

    public string CustomerId { get; set; }
}