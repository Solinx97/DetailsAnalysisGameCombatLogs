﻿namespace CombatAnalysis.Core.Models
{
    public class CombatLogByUserModel
    {
        public int Id { get; set; }

        public int CombatLogId { get; set; }

        public string UserId { get; set; }

        public int PersonalLogType { get; set; }
    }
}