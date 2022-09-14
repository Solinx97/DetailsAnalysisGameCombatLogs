using CombatAnalysis.Core.Enums;
using System;

namespace CombatAnalysis.Core.Models.Chat
{
    public class MessageModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Message { get; set; }

        public int MessageContentType { get; set; }

        public TimeSpan Time { get; set; }

        public WhenType DayTimeType { get; set; }

        public bool IsMyMessage { get; set; }

        public string OwnerId { get; set; }
    }
}
