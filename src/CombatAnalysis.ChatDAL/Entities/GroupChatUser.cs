﻿namespace CombatAnalysis.ChatDAL.Entities;

public class GroupChatUser
{
    public string Id { get; set; }

    public string Username { get; set; }

    public string CustomerId { get; set; }

    public int GroupChatId { get; set; }
}