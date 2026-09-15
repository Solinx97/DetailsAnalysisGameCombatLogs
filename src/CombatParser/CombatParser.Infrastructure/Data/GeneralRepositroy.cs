using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class GeneralRepositroy<TModel>(CombatParserContextOne context) : IGeneralRepository<TModel>
    where TModel : class, ICombatUnitRefs, IUnitTargetRefs, IGeneralEntity
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<string>> GetUniqueTargetsByCreatorIdAsync(string creatorId, CancellationToken cancellationToken, int[]? targetTypes = null)
    {
        var query = GetUniqueCreatorsQuery(creatorId, targetTypes);

        var names = await query
                     .Select(x => x.Target.Name)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync(cancellationToken);

        return names;
    }

    public async Task<IEnumerable<string>> GetUniqueCreatorsByTargetIdAsync(string targetId, CancellationToken cancellationToken, int[]? creatorTypes = null)
    {
        var query = GetUniqueTargetsQuery(targetId, creatorTypes);

        var names = await query
                     .Select(x => x.Unit.Name)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync(cancellationToken);

        return names;
    }

    public async Task<IEnumerable<string>> GetUniqueTargetSpellsByCreatorIdAsync(string creatorId, CancellationToken cancellationToken, int[]? targetTypes = null)
    {
        var query = GetUniqueCreatorsQuery(creatorId, targetTypes);

        var spells = await query
                     .Select(x => x.Spell)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync(cancellationToken);

        return spells;
    }

    public async Task<IEnumerable<string>> GetUniqueCreatorSpellsByTargetIdAsync(string targetId, CancellationToken cancellationToken, int[]? creatorTypes = null)
    {
        var query = GetUniqueTargetsQuery(targetId, creatorTypes);

        var spells = await query
                     .Select(x => x.Spell)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync(cancellationToken);

        return spells;
    }

    public async Task<IEnumerable<TModel>> GetAsync(string unitId, string target, string creator, string spell, string from, string to,
        int page, int pageSize, CancellationToken cancellationToken, int[]? targetTypes = null, int[]? creatorTypes = null)
    {
        var query = _context.Set<TModel>()
            .Include(x => x.Unit)
            .Include(x => x.Target)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(unitId))
        {
            query = query.Where(x => x.UnitId == unitId);
        }

        if (!string.IsNullOrEmpty(target))
        {
            query = query.Where(x => x.Target.Name.Equals(target));
        }

        if (!string.IsNullOrEmpty(creator))
        {
            query = query.Where(x => x.Unit.Name.Equals(creator));
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
            query = query.Where(x => creatorTypes.Contains(x.Unit.Type));
        }

        var values = await query
                     .OrderBy(x => x.Time)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync(cancellationToken);

        return values;
    }

    public async Task<IEnumerable<DamageDone>> GetDamageTakenAsync(string targetId, int combatId, string target, string creator, string spell, string from, string to,
    int page, int pageSize, CancellationToken cancellationToken, int[]? targetTypes = null, int[]? creatorTypes = null)
    {
        var query = _context.Set<DamageDone>()
            .Include(x => x.Target)
            .Include(x => x.Unit)
            .AsNoTracking()
            .AsQueryable();

        if (combatId > 0)
        {
            query = query.Where(x => x.Unit.CombatId == combatId);
        }

        if (!string.IsNullOrEmpty(targetId))
        {
            query = query.Where(x => x.TargetId == targetId);
        }

        if (!string.IsNullOrEmpty(target))
        {
            query = query.Where(x => x.Target.Name.Equals(target));
        }

        if (!string.IsNullOrEmpty(creator))
        {
            query = query.Where(x => x.Unit.Name.Equals(creator));
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
            query = query.Where(x => creatorTypes.Contains(x.Unit.Type));
        }

        var values = await query
                     .OrderBy(x => x.Time)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync(cancellationToken);

        return values;
    }

    public async Task<int> CountAsync(string unitId, string target, string creator, string spell, string from, string to, CancellationToken cancellationToken)
    {
        var query = _context.Set<TModel>()
                .Include(x => x.Target)
                .Join(_context.Set<Unit>(),
                    x => x.UnitId,
                    y => y.Id,
                    (x, y) =>
                    new
                    {
                        Value = x,
                        Creator = y
                    })
                .AsNoTracking()
                .AsQueryable();

        if (!string.IsNullOrEmpty(unitId))
        {
            query = query.Where(x => x.Value.UnitId == unitId);
        }

        if (!string.IsNullOrEmpty(target))
        {
            query = query.Where(x => x.Value.Target.Name.Equals(target));
        }

        if (!string.IsNullOrEmpty(creator))
        {
            query = query.Where(x => x.Creator.Name.Equals(creator));
        }

        if (!string.IsNullOrEmpty(spell))
        {
            query = query.Where(x => x.Value.Spell.Equals(spell));
        }

        if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
        {
            var fromTime = TimeSpan.Parse(from);
            var toTime = TimeSpan.Parse(to);
            query = query.Where(x => x.Value.Time >= fromTime && x.Value.Time <= toTime);
        }

        var count = await query
                     .CountAsync(cancellationToken);

        return count;
    }

    private IQueryable<TModel> GetUniqueTargetsQuery(string unitId, int[]? targetTypes = null)
    {
        var query = _context.Set<TModel>()
            .Include(x => x.Unit)
            .Include(x => x.Target)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(unitId))
        {
            query = query.Where(x => x.TargetId == unitId);
        }

        if (targetTypes != null && targetTypes.Length > 0)
        {
            query = query.Where(x => targetTypes.Contains(x.Target.Type));
        }

        return query;
    }

    private IQueryable<TModel> GetUniqueCreatorsQuery(string unitId, int[]? creatorTypes = null)
    {
        var query = _context.Set<TModel>()
                     .Include(x => x.Unit)
                     .Include(x => x.Target)
                     .AsNoTracking()
                     .AsQueryable();

        if (!string.IsNullOrEmpty(unitId))
        {
            query = query.Where(x => x.UnitId == unitId);
        }

        if (creatorTypes != null && creatorTypes.Length > 0)
        {
            query = query.Where(x => creatorTypes.Contains(x.Unit.Type));
        }

        return query;
    }
}
