using CombatAnalysis.CombatParserAPI.Models;
using CombatParser.Application.DTOs;

namespace CombatAnalysis.CombatParserAPI.Interfaces;

public interface ISpecializationScoreHelper
{
    Task CreateSpecializationScoreAsync(CombatPlayerModel combatPlayer, int[] spellIds, CancellationToken cancellationToken);

    Task<SpecializationScoreDto?> GetSpecializationScoreAsync(int combatPlayerId, CancellationToken cancellationToken);

    Task<BestSpecializationScoreDto?> GetBestSpecializationScoreAsync(int specId, int bossId, CancellationToken cancellationToken);

    Task UpdateSpecializationScoreAsync(int damageDone, int healDone, BestSpecializationScoreDto bestScore, SpecializationScoreDto specScore, CancellationToken cancellationToken);

    Task UpdateBestSpecializationScoreAsync(int damageDone, int healDone, BestSpecializationScoreDto bestScore, CancellationToken cancellationToken);
}
