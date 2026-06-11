using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class SpecializationRepository(CombatParserContextOne context) : ISpecializationRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<Specialization?> GetBySpellsAsync(string spells, CancellationToken cancellationToken)
    {
        var inputIds = spells
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .ToArray();

        var data = await _context.Set<Specialization>()
            .AsNoTracking()
            .FirstOrDefaultAsync(s =>
                inputIds.Any(id =>
                    ("," + s.SpecializationSpellsId + ",")
                        .Contains("," + id + ",")
                ),
                cancellationToken);

        return data;
    }
}
