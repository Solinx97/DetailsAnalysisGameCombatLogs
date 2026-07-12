using CombatParser.Domain.Data;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class GeneralRepositroy<TModel>(CombatParserContextOne context) : IGeneralRepository<TModel>
    where TModel : class, ICombatPlayerRefs, IGeneralEntity
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

    public async Task<IEnumerable<TModel>> GetAsync(int combatPlayerId, string target, string creator, string spell, string from, string to, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Set<TModel>().AsQueryable();
        if (combatPlayerId > 0)
        {
            query = query.Where(x => x.CombatPlayerId == combatPlayerId);
        }

        if (!string.IsNullOrEmpty(target))
        {
            query = query.Where(x => x.Target.Equals(target));
        }

        if (!string.IsNullOrEmpty(target))
        {
            query = query.Where(x => x.Target.Equals(target));
        }

        if (!string.IsNullOrEmpty(creator))
        {
            query = query.Where(x => x.Creator.Equals(creator));
        }

        if (!string.IsNullOrEmpty(spell))
        {
            query = query.Where(x => x.Spell.Equals(spell));
        }

        if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            var fromTime = TimeSpan.Parse(from);
            var toTime = TimeSpan.Parse(to);
            query = query.Where(x => x.Time >= fromTime && x.Time <= toTime);
        }

        var values = await query
                     .OrderBy(x => x.Time)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync(cancellationToken);

        return values;
    }

    public async Task<int> CountAsync(int combatPlayerId, string target, string creator, string spell, string from, string to, CancellationToken cancellationToken)
    {
        var query = _context.Set<TModel>().AsQueryable();
        if (combatPlayerId > 0)
        {
            query = query.Where(x => x.CombatPlayerId == combatPlayerId);
        }

        if (!string.IsNullOrEmpty(target))
        {
            query = query.Where(x => x.Target.Equals(target));
        }

        if (!string.IsNullOrEmpty(target))
        {
            query = query.Where(x => x.Target.Equals(target));
        }

        if (!string.IsNullOrEmpty(creator))
        {
            query = query.Where(x => x.Creator.Equals(creator));
        }

        if (!string.IsNullOrEmpty(spell))
        {
            query = query.Where(x => x.Spell.Equals(spell));
        }

        if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            var fromTime = TimeSpan.Parse(from);
            var toTime = TimeSpan.Parse(to);
            query = query.Where(x => x.Time >= fromTime && x.Time <= toTime);
        }

        var count = await query
                     .CountAsync(cancellationToken);

        return count;
    }
}
