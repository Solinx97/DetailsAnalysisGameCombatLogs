﻿using CombatAnalysis.DAL.Interfaces.Entities;

namespace CombatAnalysis.BL.DTO;

public class HealDoneDto : Interfaces.Entity.ICombatPlayerEntity, IGeneralFilterEntity
{
    public int Id { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public int Overheal { get; set; }

    public TimeSpan Time { get; set; }

    public string Creator { get; set; }

    public string Target { get; set; }

    public bool IsCrit { get; set; }

    public bool IsAbsorbed { get; set; }

    public int CombatPlayerId { get; set; }
}
