﻿namespace CombatAnalysis.BL.DTO.Chat
{
    public class GroupChatDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MemberNumber { get; set; }

        public int ChatPolicyType { get; set; }

        public string OwnerId { get; set; }
    }
}