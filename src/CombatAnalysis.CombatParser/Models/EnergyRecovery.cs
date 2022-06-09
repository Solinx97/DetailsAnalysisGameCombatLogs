using System;

namespace CombatAnalysis.CombatParser.Models
{
    public class EnergyRecovery
    {
        public double Value { get; set; }

        public TimeSpan Time { get; set; }

        public string SpellOrItem { get; set; }

        public int CurrentEnergy { get; set; }

        public int NowMaxEnergy { get; set; }

        public int MaxEnergy { get; set; }
    }
}
