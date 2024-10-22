﻿namespace CombatAnalysis.ChatDAL.Entities;

public class GroupChatRules
{
    public int Id { get; set; }

    public int InvitePeople { get; set; }

    public int RemovePeople { get; set; }

    public int PinMessage { get; set; }

    public int Announcements { get; set; }

    public int GroupChatId { get; set; }
}
