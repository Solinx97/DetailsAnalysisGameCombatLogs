using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class CommunityRepository(CommunicationContext context) : ICommunityRepository
{
    private readonly CommunicationContext _context = context;

    public async Task DeleteAsync(int id, CancellationToken cancelationToken)
    {
        await _context.Set<Community>()
            .Where(cl => cl.Id == id)
            .ExecuteDeleteAsync(cancelationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        var count = await _context.Set<Community>().CountAsync(cancellationToken);

        return count;
    }
}
