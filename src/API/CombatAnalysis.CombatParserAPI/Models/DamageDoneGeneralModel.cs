﻿namespace CombatAnalysis.CombatParserAPI.Models
{
    public class DamageDoneGeneralModel
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public double DamagePerSecond { get; set; }

        public string SpellOrItem { get; set; }

        public int CritNumber { get; set; }

        public int MissNumber { get; set; }

        public int CastNumber { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public double AverageValue { get; set; }

        public int CombatPlayerDataId { get; set; }
    }
}