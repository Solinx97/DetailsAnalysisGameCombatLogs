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
                     .OrderBy(x => x)
                     .Distinct()
                     .ToListAsync();

        return uniqueValues;
    }

    public async Task<int> CountTargetsByCombatPlayerIdAsync(int combatPlayerId, string target)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target));

        return count;
    }

    public async Task<IEnumerable<TModel>> GetTargetsByCombatPlayerIdAsync(int combatPlayerId, string target, int page, int pageSize)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target))
                     .OrderBy(x => x.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync();

        return values;
    }
}
