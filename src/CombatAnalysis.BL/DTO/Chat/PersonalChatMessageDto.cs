﻿using System;

namespace CombatAnalysis.BL.DTO.Chat
{
    public class PersonalChatMessageDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Message { get; set; }

        public TimeSpan Time { get; set; }

        public int PersonalChatId { get; set; }
    }
}