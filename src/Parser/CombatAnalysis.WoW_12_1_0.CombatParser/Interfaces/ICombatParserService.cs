using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW_12_1_0.CombatParser.Details;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Interfaces;

public interface ICombatParserService
{
    List<Combat> Combats { get; }

    List<CombatDetails> CombatDetails { get; }

    Task<bool> FileCheckAsync(string combatLog);

    Task ParseAsync(List<string> combatLogPaths, CancellationToken cancellationToken);

    void Clear();
}
