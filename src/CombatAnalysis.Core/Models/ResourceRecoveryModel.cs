using System;

namespace CombatAnalysis.Core.Models
{
    public class ResourceRecoveryModel
    {
        public double Value { get; set; }

        public TimeSpan Time { get; set; }

        public string SpellOrItem { get; set; }
    }
}
