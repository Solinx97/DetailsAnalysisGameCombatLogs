using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using CombatParser.Application.Queries.GetSpecializationBySpell;
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
}
