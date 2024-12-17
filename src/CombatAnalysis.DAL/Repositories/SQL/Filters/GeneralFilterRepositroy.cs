using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Filters;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.SQL.Filters;

internal class GeneralFilterRepositroy<TModel> : IGeneralFilter<TModel>
    where TModel : class, IGeneralFilterEntity, ICombatPlayerEntity
{
    private readonly CombatParserSQLContext _context;

    public GeneralFilterRepositroy(CombatParserSQLContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<string>> GetTargetNamesByCombatPlayerIdAsync(int combatPlayerId)
    {
        var uniqueValues = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId)
                     .Select(x => x.Target)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync();

        return uniqueValues;
    }

    public async Task<int> CountTargetByCombatPlayerIdAsync(int combatPlayerId, string target)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target));

        return count;
    }

    public async Task<IEnumerable<TModel>> GetTargetByCombatPlayerIdAsync(int combatPlayerId, string target, int page, int pageSize)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target))
                     .OrderBy(x => x.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync();

        return values;
    }

    public async Task<IEnumerable<string>> GetCreatorNamesByCombatPlayerIdAsync(int combatPlayerId)
    {
        var uniqueValues = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId)
                     .Select(x => x.Creator)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync();

        return uniqueValues;
    }

    public async Task<int> CountCreatorByCombatPlayerIdAsync(int combatPlayerId, string creator)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId && x.Creator.Equals(creator));

        return count;
    }

    public async Task<IEnumerable<TModel>> GetCreatorByCombatPlayerIdAsync(int combatPlayerId, string creator, int page, int pageSize)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Creator.Equals(creator))
                     .OrderBy(x => x.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync();

        return values;
    }
}
