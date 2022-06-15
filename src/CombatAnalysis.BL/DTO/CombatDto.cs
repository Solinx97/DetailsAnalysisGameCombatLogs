using System;
using System.Collections.Generic;

namespace CombatAnalysis.BL.DTO
{
    public class CombatDto
    {
        public string DungeonName { get; set; }

        public string Name { get; set; }

        public List<string> Data { get; set; }

        public double EnergyRecovery { get; set; }

        public int DamageDone { get; set; }

        public int HealDone { get; set; }

        public int DamageTaken { get; set; }

        public int DeathNumber { get; set; }

        public int UsedBuffs { get; set; }

        public bool IsWin { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset FinishDate { get; set; }

        //public List<PlayerCombat> Players { get; set; }

        public string Duration
        {
            get { return (FinishDate - StartDate).ToString(@"hh\:mm\:ss"); }
        }
    }
}
