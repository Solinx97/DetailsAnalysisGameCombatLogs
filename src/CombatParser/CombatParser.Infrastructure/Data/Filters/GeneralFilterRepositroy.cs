using CombatParser.Domain.Data.Filters;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data.Filters;

internal class GeneralFilterRepositroy<TModel>(CombatParserContextOne context) : IGeneralFilterRepository<TModel>
    where TModel : class, ICombatPlayerRefs, IGeneralFilterEntity
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<string>> GetUniqueTargetsByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueValues = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId)
                     .Select(x => x.Target)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync(cancellationToken);

        return uniqueValues;
    }

    public async Task<int> CountDamageDoneByTargetAsync(int combatPlayerId, string target, CancellationToken cancellationToken)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target), cancellationToken);

        return count;
    }

    public async Task<IEnumerable<TModel>> GetByTargetAsync(int combatPlayerId, string target, int page, int pageSize, CancellationToken cancellationToken)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target))
                     .OrderBy(x => x.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync(cancellationToken);

        return values;
    }

    public async Task<int> GetValueToTargetByCombatPlayerIdAsync(int combatPlayerId, string target, CancellationToken cancellationToken)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Target.Equals(target))
                     .OrderBy(x => x.Id)
                     .SumAsync(x => x.Value, cancellationToken);

        return values;
    }

    public async Task<IEnumerable<string>> GetCreatorNamesByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueValues = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId)
                     .Select(x => x.Creator)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync(cancellationToken);

        return uniqueValues;
    }

    public async Task<int> CountCreatorByCombatPlayerIdAsync(int combatPlayerId, string creator, CancellationToken cancellationToken)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId && x.Creator.Equals(creator), cancellationToken);

        return count;
    }

    public async Task<IEnumerable<TModel>> GetByCreatorAsync(int combatPlayerId, string creator, int page, int pageSize, CancellationToken cancellationToken)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Creator.Equals(creator))
                     .OrderBy(x => x.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync(cancellationToken);

        return values;
    }

    public async Task<IEnumerable<string>> GetUniqueSpellsByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var uniqueValues = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId)
                     .Select(x => x.Spell)
                     .Distinct()
                     .OrderBy(x => x)
                     .ToListAsync(cancellationToken);

        return uniqueValues;
    }

    public async Task<int> CountDamageDoneBySpellAsync(int combatPlayerId, string spell, CancellationToken cancellationToken)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId && x.Spell.Equals(spell), cancellationToken);

        return count;
    }

    public async Task<IEnumerable<TModel>> GetBySpellAsync(int combatPlayerId, string spell, int page, int pageSize, CancellationToken cancellationToken)
    {
        var values = await _context.Set<TModel>()
                     .Where(x => x.CombatPlayerId == combatPlayerId && x.Spell.Equals(spell))
                     .OrderBy(x => x.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync(cancellationToken);

        return values;
    }
}
