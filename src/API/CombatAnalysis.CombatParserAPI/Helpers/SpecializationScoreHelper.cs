using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using CombatParser.Application.Commands.UpdateBestSpecializationScore;
using CombatParser.Application.Commands.UpdateSpecializationScore;
using CombatParser.Application.DTOs;
using CombatParser.Application.Queries.GetBestSpecializationScore;
using CombatParser.Application.Queries.GetSpecializationBySpell;
using CombatParser.Application.Queries.GetSpecializationScore;
using MediatR;

namespace CombatAnalysis.CombatParserAPI.Helpers;

internal class SpecializationScoreHelper(IMediator mediator) : ISpecializationScoreHelper
{
    private readonly IMediator _mediator = mediator;

    public async Task CreateSpecializationScoreAsync(CombatPlayerModel combatPlayer, int[] spellsId, CancellationToken cancellationToken)
    {
        var spellsIdAsString = string.Join(',', spellsId);

        var spec = await _mediator.Send(new GetSpecializationBySpellQuery(spellsIdAsString), cancellationToken);
        if (spec == null)
        {
            return;
        }

        var score = new SpecializationScoreModel
        {
            DamageDone = combatPlayer.DamageDone,
            HealDone = combatPlayer.HealDone,
            SpecializationId = spec.Id,
        };

        combatPlayer.Score = score;
    }

    public async Task<SpecializationScoreDto?> GetSpecializationScoreAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var specScores = await _mediator.Send(new GetSpecializationScoreQuery(combatPlayerId), cancellationToken);

        return specScores;
    }

    public async Task<BestSpecializationScoreDto?> GetBestSpecializationScoreAsync(int specId, int bossId, CancellationToken cancellationToken)
    {
        var bestScore = await _mediator.Send(new GetBestSpecializationScoreQuery(specId, bossId), cancellationToken);

        return bestScore;
    }

    public async Task UpdateSpecializationScoreAsync(int damageDone, int healDone, BestSpecializationScoreDto bestScore, SpecializationScoreDto specScore, CancellationToken cancellationToken)
    {
        if (bestScore.DamageDone < damageDone)
        {
            specScore.DamageScore = 100;
        }
        else
        {
            specScore.DamageScore = damageDone == 0 ? 0 : ((double)damageDone / (double)bestScore.DamageDone) * 100;
        }

        if (bestScore.HealDone < healDone)
        {
            specScore.HealScore = 100;
        }
        else
        {
            specScore.HealScore = healDone == 0 ? 0 : ((double)healDone / (double)bestScore.HealDone) * 100;
        }

        await _mediator.Send(new UpdateSpecializationScoreCommand(specScore.Id, specScore.DamageScore, specScore.HealScore), cancellationToken);
    }

    public async Task UpdateBestSpecializationScoreAsync(int damageDone, int healDone, BestSpecializationScoreDto bestScore, CancellationToken cancellationToken)
    {
        var updatedBestScore = new BestSpecializationScoreDto
        {
            Id = bestScore.Id,
            SpecializationId = bestScore.SpecializationId,
            BossId = bestScore.BossId,
        };

        if (bestScore.DamageDone < damageDone)
        {
            updatedBestScore.DamageDone = damageDone;
        }

        if (bestScore.HealDone < healDone)
        {
            updatedBestScore.HealDone = healDone;
        }

        await _mediator.Send(new UpdateBestSpecializationScoreQuery(updatedBestScore.Id, updatedBestScore.DamageDone, updatedBestScore.HealDone), cancellationToken);
    }
}
