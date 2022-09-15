using System;

namespace CombatAnalysis.ChatApi.Models
{
    public class MessageDataModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Message { get; set; }

        public bool IsMyMessage { get; set; }

        public string Time { get; set; }

        public int DayTimeType { get; set; }

        public int ChatMessageId { get; set; }
    }
}
