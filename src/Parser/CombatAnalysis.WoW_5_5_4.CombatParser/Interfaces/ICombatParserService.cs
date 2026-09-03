using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW_5_5_4.CombatParser.Details;

namespace CombatAnalysis.WoW_5_5_4.CombatParser.Interfaces;

public interface ICombatParserService
{
    List<Combat> Combats { get; }

    List<CombatDetails> CombatDetails { get; }

    Task<bool> FileCheckAsync(string combatLog);

    Task ParseAsync(List<string> combatLogPaths, CancellationToken cancellationToken);

    void Clear();
}
