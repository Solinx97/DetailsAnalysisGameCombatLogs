using CombatParser.Domain.Data.Filters;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data.Filters;

internal class GeneralFilterRepositroy<TModel>(CombatParserContextOne context) : IGeneralFilterRepository<TModel>
    where TModel : class, ICombatPlayerRefs, IGeneralFilterEntity
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<string>> GetUniqueTargetsAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueTargets = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId)
                     .Select(x => x.Target)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync(cancellationToken);

        return uniqueTargets;
    }

    public async Task<int> CountByTargetAsync(int combatPlayerId, string target, CancellationToken cancellationToken)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target), cancellationToken);

        return count;
    }

    public async Task<IEnumerable<TModel>> GetByTargetAsync(int combatPlayerId, string target, int page, int pageSize, CancellationToken cancellationToken)
    {
        var result = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target))
                     .OrderBy(x => x.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<int> GetValueToTargetAsync(int combatPlayerId, string target, CancellationToken cancellationToken)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target))
                     .OrderBy(x => x.Id)
                     .SumAsync(x => x.Value, cancellationToken);

        return values;
    }

    public async Task<IEnumerable<string>> GetCreatorNamesAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueCreatorNames = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId)
                     .Select(x => x.Creator)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync(cancellationToken);

        return uniqueCreatorNames;
    }

    public async Task<int> CountByCreatorAsync(int combatPlayerId, string creator, CancellationToken cancellationToken)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId && x.Creator.Equals(creator), cancellationToken);

        return count;
    }

    public async Task<IEnumerable<TModel>> GetByCreatorAsync(int combatPlayerId, string creator, int page, int pageSize, CancellationToken cancellationToken)
    {
        var result = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Creator.Equals(creator))
                     .OrderBy(x => x.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<IEnumerable<string>> GetUniqueSpellsAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueSpells = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId)
                     .Select(x => x.Spell)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync(cancellationToken);

        return uniqueSpells;
    }

    public async Task<IEnumerable<TModel>> GetBySpellAsync(int combatPlayerId, string spell, int page, int pageSize, CancellationToken cancellationToken)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Spell.Equals(spell))
                     .OrderBy(x => x.Time)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync(cancellationToken);

        return values;
    }

    public async Task<int> CountBySpellAsync(int combatPlayerId, string spell, CancellationToken cancellationToken)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId && x.Spell.Equals(spell), cancellationToken);

        return count;
    }

    public async Task<IEnumerable<TModel>> GetByAllTargetsAsync(int combatPlayerId, string target, string spell, int page, int pageSize, CancellationToken cancellationToken)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target) && x.Spell.Equals(spell))
                     .OrderBy(x => x.Time)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync(cancellationToken);

        return values;
    }

    public async Task<IEnumerable<TModel>> GetByAllCreatorsAsync(int combatPlayerId, string creator, string spell, int page, int pageSize, CancellationToken cancellationToken)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Creator.Equals(creator) && x.Spell.Equals(spell))
                     .OrderBy(x => x.Time)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync(cancellationToken);

        return values;
    }

    public async Task<int> CountByAllTargetsAsync(int combatPlayerId, string target, string spell, CancellationToken cancellationToken)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target) && x.Spell.Equals(spell), cancellationToken);

        return count;
    }

    public async Task<int> CountByAllCreatorsAsync(int combatPlayerId, string creator, string spell, CancellationToken cancellationToken)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId && x.Creator.Equals(creator) && x.Spell.Equals(spell), cancellationToken);

        return count;
    }
}
