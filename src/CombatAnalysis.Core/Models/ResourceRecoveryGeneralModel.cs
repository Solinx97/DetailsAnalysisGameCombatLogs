﻿namespace CombatAnalysis.Core.Models;

public class ResourceRecoveryGeneralModel
{
    public int Id { get; set; }

    public int Value { get; set; }

    public double ResourcePerSecond { get; set; }

    public string SpellOrItem { get; set; }

    public int CastNumber { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }

    public double AverageValue { get; set; }

    public int CombatPlayerId { get; set; }
}
