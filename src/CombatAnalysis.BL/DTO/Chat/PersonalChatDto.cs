using System;

namespace CombatAnalysis.BL.DTO.Chat
{
    public class PersonalChatDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string LastMessage { get; set; }

        public int MessageContentType { get; set; }

        public TimeSpan Time { get; set; }

        public int IsNotReadMessageNumber { get; set; }
    }
}
