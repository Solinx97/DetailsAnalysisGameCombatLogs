﻿namespace CombatAnalysis.WebApp.Models;

public class CombatPlayerPositionModel
{
    public int Id { get; set; }

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatPlayerId { get; set; }

    public int CombatId { get; set; }
}
