﻿namespace CombatAnalysis.ChatDAL.Entities;

public class GroupChatMessage
{
    public int Id { get; set; }

    public string Message { get; set; }

    public DateTimeOffset Time { get; set; }

    public int Status { get; set; }

    public int Type { get; set; }

    public int GroupChatId { get; set; }

    public string AppUserId { get; set; }
}
