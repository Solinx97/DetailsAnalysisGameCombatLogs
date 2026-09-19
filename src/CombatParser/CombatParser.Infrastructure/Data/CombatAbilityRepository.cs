using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Infrastructure.Enums;
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

            join aura in _context.Set<UnitAura>().AsNoTracking()
                on ability.GameId equals aura.GameAuraId

            join unit in _context.Set<Unit>().AsNoTracking()
                on aura.UnitId equals unit.Id

            join player in _context.Set<CombatPlayer>().AsNoTracking()
                on unit.Id equals player.UnitId

            join combatEntity in _context.Set<Combat>().AsNoTracking()
                on player.CombatId equals combatEntity.Id

            where combatEntity.Id == combat.CombatId

            where (
                (player.Id == combatPlayerId && aura.AuraType == 0)
                || (unit.Name == combat.Player.Username && aura.AuraType == 1)
            )

            select ability
        ).ToListAsync(cancellationToken);

        return abilities;
    }

    public async Task<Dictionary<string, int>> GetPotionsAsync(int combatLogId, CancellationToken cancellationToken)
    {
        var potions = await (
            from ability in _context.Set<CombatAbility>().AsNoTracking()
            where ability.AbilityType == (int)CombatAbilityType.EfficiencyPotion

            join aura in _context.Set<UnitAura>().AsNoTracking()
                on ability.GameId equals aura.GameAuraId

            join unit in _context.Set<Unit>().AsNoTracking()
                on aura.UnitId equals unit.Id

            join player in _context.Set<CombatPlayer>().AsNoTracking()
                on unit.Id equals player.UnitId

            join combatEntity in _context.Set<Combat>().AsNoTracking()
                on unit.CombatId equals combatEntity.Id

            where combatEntity.CombatLogId == combatLogId

            select new { Username = player.Player.Username, Ability = ability.Id }
        )
        .GroupBy(x => x.Username)
        .ToDictionaryAsync(g => g.Key, g => g.Count(), cancellationToken);

        return potions;
    }

    public async Task<IEnumerable<PreAuraEnchanced>> GetByPreAuraAsync(int combatId, CancellationToken cancellationToken)
    {
        var preAuras = await (
            from ability in _context.Set<CombatAbility>().AsNoTracking()

            join preAura in _context.Set<UnitPreAura>().AsNoTracking()
                on ability.GameId equals preAura.GameId

            join unit in _context.Set<Unit>().AsNoTracking()
                on preAura.UnitId equals unit.Id

            join combatEntity in _context.Set<Combat>().AsNoTracking()
                on unit.CombatId equals combatEntity.Id

            where combatEntity.Id == combatId

            select new PreAuraEnchanced(preAura.Id, unit.GameId, preAura.GameId, ability.Name, ability.AbilityType, preAura.Status, unit.Id)
        ).Distinct().ToListAsync(cancellationToken);

        return preAuras;
    }

    public async Task<IEnumerable<PreAuraEnchanced>> GetByPreAuraAsync(int combatId, string unitId, CancellationToken cancellationToken)
    {
        var preAuras = await (
            from ability in _context.Set<CombatAbility>().AsNoTracking()

            join preAura in _context.Set<UnitPreAura>().AsNoTracking()
                on ability.GameId equals preAura.GameId

            join unit in _context.Set<Unit>().AsNoTracking()
                on preAura.UnitId equals unit.Id

            join combatEntity in _context.Set<Combat>().AsNoTracking()
                on unit.CombatId equals combatEntity.Id

            where combatEntity.Id == combatId
                && preAura.UnitId == unitId

            select new PreAuraEnchanced(preAura.Id, unit.GameId, preAura.GameId, ability.Name, ability.AbilityType, preAura.Status, unit.Id)
        ).Distinct().ToListAsync(cancellationToken);

        return preAuras;
    }
}
