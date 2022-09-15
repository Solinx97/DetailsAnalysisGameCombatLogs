using System;

namespace CombatAnalysis.Core.Models.Chat
{
    public class MessageDataModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Message { get; set; }

        public bool IsMyMessage { get; set; }

        public TimeSpan Time { get; set; }

        public int DayTimeType { get; set; }

        public int ChatMessageId { get; set; }
    }
}
