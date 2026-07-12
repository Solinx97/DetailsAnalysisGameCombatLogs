using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class PlayerRepository(CombatParserContextOne context) : IPlayerRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<Player?> GetByGameIdAsync(string gameId, CancellationToken cancellationToken)
    {
        var player = await _context.Set<Player>()
             .SingleOrDefaultAsync(b => b.GameId == gameId, cancellationToken);

        return player;
    }
}
