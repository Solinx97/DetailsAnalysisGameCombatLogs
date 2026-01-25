using CombatAnalysis.CombatParser.Details;
using CombatAnalysis.CombatParser.Entities;

namespace CombatAnalysis.CombatParser.Interfaces;

public interface ICombatParserService
{
    List<Combat> Combats { get; }

    List<CombatDetails> CombatDetails { get; }

    Task<bool> FileCheckAsync(string combatLog);

    Task ParseAsync(List<string> combatLogPaths, CancellationToken cancellationToken);

    void Clear();
}
