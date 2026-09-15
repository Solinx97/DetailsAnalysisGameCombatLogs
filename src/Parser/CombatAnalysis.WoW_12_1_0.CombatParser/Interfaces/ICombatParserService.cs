using CombatAnalysis.WoW.CombatParser.Entities;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Interfaces;

public interface ICombatParserService
{
    List<Combat> Combats { get; }

    Task<bool> FileCheckAsync(string combatLog);

    Task ParseAsync(List<string> combatLogPaths, CancellationToken cancellationToken);

    void Clear();
}
