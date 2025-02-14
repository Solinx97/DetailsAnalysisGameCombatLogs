﻿using CombatAnalysis.CombatParser.Entities;

namespace CombatAnalysis.CombatParser.Interfaces;

public interface IParser<TModel> : IObservable<TModel>
    where TModel : class
{
    List<Combat> Combats { get; }

    Dictionary<string, List<string>> PetsId { get; }

    Task<bool> FileCheckAsync(string combatLog);

    Task ParseAsync(string combatLog);

    void Clear();
}