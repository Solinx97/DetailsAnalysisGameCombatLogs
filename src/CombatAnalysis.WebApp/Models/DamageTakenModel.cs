﻿namespace CombatAnalysis.WebApp.Models
{
    public class DamageTakenModel
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public string Time { get; set; }

        public string FromEnemy { get; set; }

        public string ToPlayer { get; set; }

        public string SpellOrItem { get; set; }

        public bool IsDodge { get; set; }

        public bool IsParry { get; set; }

        public bool IsMiss { get; set; }

        public bool IsResist { get; set; }

        public bool IsImmune { get; set; }

        public bool IsCrushing { get; set; }

        public int CombatPlayerId { get; set; }
    }
}