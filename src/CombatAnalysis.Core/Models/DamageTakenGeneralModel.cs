﻿namespace CombatAnalysis.Core.Models
{
    public class DamageTakenGeneralModel
    {
        public int Value { get; set; }

        public double DamageTakenPerSecond { get; set; }

        public string SpellOrItem { get; set; }

        public int CritNumber { get; set; }

        public int MissNumber { get; set; }

        public int CastNumber { get; set; }

        public int MinValue { get; set; }

        public int MaxValue { get; set; }

        public double AverageValue { get; set; }
    }
}