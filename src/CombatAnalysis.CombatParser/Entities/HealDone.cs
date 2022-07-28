using System;

namespace CombatAnalysis.CombatParser.Entities
{
    public class HealDone
    {
        public int ValueWithOverheal { get; set; }

        public TimeSpan Time { get; set; }

        public int Overheal { get; set; }

        public int Value { get { return ValueWithOverheal - Overheal; } }

        public string FromPlayer { get; set; }

        public string ToPlayer { get; set; }

        public string SpellOrItem { get; set; }

        public int CurrentHealth { get; set; }

        public int MaxHealth { get; set; }

        public bool IsCrit { get; set; }

        public bool IsFullOverheal { get; set; }
    }
}