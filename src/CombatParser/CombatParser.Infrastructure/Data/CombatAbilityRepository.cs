using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatAbilityRepository(CombatParserContextOne context) : ICombatAbilityRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<CombatAbility>> GetByAbilityTypeAsync(int combatPlayerId, int[] abilityTypes, CancellationToken cancellationToken)
    {
        var combat = await _context.Set<CombatPlayer>()
            .AsNoTracking()
            .Include(x => x.Player)
            .FirstAsync(x => x.Id == combatPlayerId, cancellationToken);

        var abilities = await (
            from ability in _context.Set<CombatAbility>().AsNoTracking()
            where abilityTypes.Contains(ability.AbilityType)

            join aura in _context.Set<CombatPlayerAura>().AsNoTracking()
                on ability.GameId equals aura.GameAuraId

            join player in _context.Set<CombatPlayer>().AsNoTracking()
                on aura.CombatPlayerId equals player.Id

            join combatEntity in _context.Set<Combat>().AsNoTracking()
                on player.CombatId equals combatEntity.Id

            where combatEntity.Id == combat.CombatId

            where (
                (player.Id == combatPlayerId && aura.AuraType == 0)
                || (aura.Target == combat.Player.Username && aura.AuraType == 1)
            )

            select ability
        ).ToListAsync(cancellationToken);

        return abilities;
    }
}
