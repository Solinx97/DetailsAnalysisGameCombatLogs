﻿namespace CombatAnalysis.WebApp.Models.Chats;

public class PersonalChatMessageModel
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Message { get; set; }

    public string Time { get; set; }

    public int PersonalChatId { get; set; }
}