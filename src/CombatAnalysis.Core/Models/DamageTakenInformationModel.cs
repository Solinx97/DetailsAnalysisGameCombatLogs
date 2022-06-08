using System;

namespace CombatAnalysis.Core.Models
{
    public class DamageTakenInformationModel
    {
        public int Value { get; set; }

        public TimeSpan Time { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string SpellOrItem { get; set; }

        public bool IsDodge { get; set; }

        public bool IsParry { get; set; }

        public bool IsMiss { get; set; }

        public bool IsResist { get; set; }

        public bool IsImmune { get; set; }

        public bool IsCrushing { get; set; }
    }
}
