﻿namespace CombatAnalysis.ChatApi.Models
{
    public class PersonalChatModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Message { get; set; }

        public string Time { get; set; }

        public int PersonalChatId { get; set; }
    }
}