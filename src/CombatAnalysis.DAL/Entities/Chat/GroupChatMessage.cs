using System;

namespace CombatAnalysis.DAL.Entities.Chat
{
    public class GroupChatMessage
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Message { get; set; }

        public TimeSpan Time { get; set; }

        public int GroupChatId { get; set; }
    }
}
