﻿namespace CombatAnalysis.ChatDAL.Entities;

public class PersonalChatMessage
{
    public int Id { get; set; }

    public string Message { get; set; }

    public DateTimeOffset Time { get; set; }

    public int Status { get; set; }

    public int Type { get; set; }

    public int PersonalChatId { get; set; }

    public string AppUserId { get; set; }
}
