using CombatParser.Domain.Data;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class GeneralRepositroy<TModel>(CombatParserContextOne context) : IGeneralRepository<TModel>
    where TModel : class, ICombatPlayerRefs, IGeneralEntity
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<string>> GetUniqueTargetsAsync(int combatPlayerId, CancellationToken cancellationToken, int[]? targetTypes = null)
    {
        var query = _context.Set<TModel>()
                     .Include(x => x.Target)
                     .AsNoTracking()
                     .AsQueryable();

        if (combatPlayerId > 0)
        {
            query = query.Where(x => x.CombatPlayerId == combatPlayerId);
        }

        if (targetTypes != null && targetTypes.Length > 0)
        {
            query = query.Where(x => targetTypes.Contains(x.Target.Type));
        }

        var uniqueTargets = await query
                     .Select(x => x.Target.Name)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync(cancellationToken);

        return uniqueTargets;
    }

    public async Task<IEnumerable<string>> GetCreatorNamesAsync(int combatPlayerId, CancellationToken cancellationToken, int[]? creatorTypes = null)
    {
        var query = _context.Set<TModel>()
                     .Include(x => x.Creator)
                     .AsNoTracking()
                     .AsQueryable();

        if (combatPlayerId > 0)
        {
            query = query.Where(x => x.CombatPlayerId == combatPlayerId);
        }

        if (creatorTypes != null && creatorTypes.Length > 0)
        {
            query = query.Where(x => creatorTypes.Contains(x.Creator.Type));
        }

        var uniqueCreatorNames = await query
                     .Select(x => x.Creator.Name)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync(cancellationToken);

        return uniqueCreatorNames;
    }

    public async Task<IEnumerable<string>> GetUniqueSpellsAsync(int combatPlayerId, CancellationToken cancellationToken, int[]? targetTypes = null, int[]? creatorTypes = null)
    {
        var query = _context.Set<TModel>()
            .Include(x => x.Target)
            .Include(x => x.Creator)
            .AsNoTracking()
            .AsQueryable();

        if (targetTypes != null && targetTypes.Length > 0)
        {
            query = query.Where(x => targetTypes.Contains(x.Target.Type));
        }

        if (creatorTypes != null && creatorTypes.Length > 0)
        {
            query = query.Where(x => creatorTypes.Contains(x.Creator.Type));
        }

        var uniqueSpells = await query
                     .Where(x => x.CombatPlayerId == combatPlayerId)
                     .Select(x => x.Spell)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync(cancellationToken);

        return uniqueSpells;
    }

    public async Task<IEnumerable<TModel>> GetAsync(int combatPlayerId, string target, string creator, string spell, string from, string to,
        int page, int pageSize, CancellationToken cancellationToken, int[]? targetTypes = null, int[]? creatorTypes = null)
    {
        var query = _context.Set<TModel>()
            .Include(x => x.Target)
            .Include(x => x.Creator)
            .AsNoTracking()
            .AsQueryable();

        if (combatPlayerId > 0)
        {
            query = query.Where(x => x.CombatPlayerId == combatPlayerId);
        }

        if (!string.IsNullOrEmpty(target))
        {
            query = query.Where(x => x.Target.Name.Equals(target));
        }

        if (!string.IsNullOrEmpty(creator))
        {
            query = query.Where(x => x.Creator.Name.Equals(creator));
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

        if (targetTypes != null && targetTypes.Length > 0)
        {
            query = query.Where(x => targetTypes.Contains(x.Target.Type));
        }

        if (creatorTypes != null && creatorTypes.Length > 0)
        {
            query = query.Where(x => creatorTypes.Contains(x.Creator.Type));
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
        var query = _context.Set<TModel>()
            .AsNoTracking()
            .AsQueryable();

        if (combatPlayerId > 0)
        {
            query = query.Where(x => x.CombatPlayerId == combatPlayerId);
        }

        if (!string.IsNullOrEmpty(target))
        {
            query = query.Where(x => x.Target.Name.Equals(target));
        }

        if (!string.IsNullOrEmpty(creator))
        {
            query = query.Where(x => x.Creator.Name.Equals(creator));
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
