﻿namespace CombatAnalysis.DAL.Entities.Chat;

public class PersonalChatMessageCount
{
    public int Id { get; set; }

    public int Count { get; set; }

    public string CustomerId { get; set; }

    public int PersonalChatId { get; set; }
}