using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class GeneralRepositroy<TModel>(CombatParserContextOne context) : IGeneralRepository<TModel>
    where TModel : class, ICombatUnitRefs, IUnitTargetRefs, IGeneralEntity
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<string>> GetUniqueTargetsAsync(string unitId, CancellationToken cancellationToken, int[]? targetTypes = null)
    {
        var query = _context.Set<TModel>()
                     .Include(x => x.Target)
                     .AsNoTracking()
                     .AsQueryable();

        if (!string.IsNullOrEmpty(unitId))
        {
            query = query.Where(x => x.UnitId == unitId);
        }

        if (targetTypes != null && targetTypes.Length > 0)
        {
            //query = query.Where(x => targetTypes.Contains(x.Value.Target.Type));
        }

        var uniqueTargets = await query
                     .Select(x => x.Target.Name)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync(cancellationToken);

        return uniqueTargets;
    }

    public async Task<IEnumerable<string>> GetCreatorNamesAsync(string unitId, CancellationToken cancellationToken, int[]? creatorTypes = null)
    {
        var query = _context.Set<TModel>()
                     .Include(x => x.Target)
                     .Join(_context.Set<Unit>(),
                        x => x.UnitId,
                        y => y.Id,
                        (x, y) =>
                        new {
                            Value = x,
                            Creator = y
                        })
                     .AsNoTracking()
                     .AsQueryable();

        if (!string.IsNullOrEmpty(unitId))
        {
            query = query.Where(x => x.Value.UnitId == unitId);
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

    public async Task<IEnumerable<string>> GetUniqueSpellsAsync(string unitId, CancellationToken cancellationToken, int[]? targetTypes = null, int[]? creatorTypes = null)
    {
        var query = _context.Set<TModel>()
            .Include(x => x.Target)
            .Join(_context.Set<Unit>(),
                x => x.UnitId,
                y => y.Id,
                (x, y) =>
                new {
                    Value = x,
                    Creator = y
                })
            .AsNoTracking()
            .AsQueryable();

        if (targetTypes != null && targetTypes.Length > 0)
        {
            query = query.Where(x => targetTypes.Contains(x.Value.Target.Type));
        }

        if (creatorTypes != null && creatorTypes.Length > 0)
        {
            query = query.Where(x => creatorTypes.Contains(x.Creator.Type));
        }

        var uniqueSpells = await query
                     .Where(x => x.Value.UnitId == unitId)
                     .Select(x => x.Value.Spell)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync(cancellationToken);

        return uniqueSpells;
    }

    public async Task<IEnumerable<TModel>> GetAsync(string unitId, string target, string creator, string spell, string from, string to,
        int page, int pageSize, CancellationToken cancellationToken, int[]? targetTypes = null, int[]? creatorTypes = null)
    {
        var query = _context.Set<TModel>()
            .Include(x => x.Target)
            .Join(_context.Set<Unit>(),
                x => x.UnitId,
                y => y.Id,
                (x, y) =>
                new {
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

        if (targetTypes != null && targetTypes.Length > 0)
        {
            query = query.Where(x => targetTypes.Contains(x.Value.Target.Type));
        }

        if (creatorTypes != null && creatorTypes.Length > 0)
        {
            query = query.Where(x => creatorTypes.Contains(x.Creator.Type));
        }

        var values = await query
                     .OrderBy(x => x.Value.Time)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .Select(x => x.Value)
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
                    new {
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
}
