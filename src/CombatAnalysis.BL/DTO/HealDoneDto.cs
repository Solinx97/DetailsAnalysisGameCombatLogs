﻿using System;

namespace CombatAnalysis.BL.DTO
{
    public class HealDoneDto
    {
        public int Id { get; set; }

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

        public int CombatPlayerDataId { get; set; }
    }
}