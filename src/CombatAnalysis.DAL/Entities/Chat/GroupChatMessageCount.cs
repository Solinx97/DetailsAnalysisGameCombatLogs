﻿namespace CombatAnalysis.DAL.Entities.Chat;

public class GroupChatMessageCount
{
    public int Id { get; set; }

    public int Count { get; set; }

    public string CustomerId { get; set; }

    public int GroupChatId { get; set; }
}