using System;

namespace CombatAnalysis.Core.Models.Chat
{
    public class GroupChatMessageModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Message { get; set; }

        public TimeSpan Time { get; set; }

        public int GroupChatId { get; set; }
    }
}
