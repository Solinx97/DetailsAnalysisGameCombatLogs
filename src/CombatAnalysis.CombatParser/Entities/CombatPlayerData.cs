﻿namespace CombatAnalysis.CombatParser.Entities
{
    public class CombatPlayerData
    {
        public string UserName { get; set; }

        public double EnergyRecovery { get; set; }

        public int DamageDone { get; set; }

        public int HealDone { get; set; }

        public int DamageTaken { get; set; }

        public int UsedBuffs { get; set; }
    }
}