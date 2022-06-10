using System;

namespace CombatAnalysis.CombatParser.Entities
{
    public class ResourceRecovery
    {
        public double Value { get; set; }

        public TimeSpan Time { get; set; }

        public string SpellOrItem { get; set; }
    }
}
