﻿namespace CombatAnalysis.ChatBL.DTO;

public class GroupChatMessageCountDto
{
    public int Id { get; set; }

    public int Count { get; set; }

    public string GroupChatUserId { get; set; }

    public int GroupChatId { get; set; }
}
