﻿namespace CombatAnalysis.Core.Models.Chat;

public class GroupChatMessageModel
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Message { get; set; }

    public DateTimeOffset Time { get; set; }

    public int Status { get; set; }

    public int Type { get; set; }

    public int ChatId { get; set; }

    public string GroupChatUserId { get; set; }
}
