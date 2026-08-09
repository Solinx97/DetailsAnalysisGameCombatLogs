using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class UserPostRepository(CommunicationContext context) : IUserPostRepository
{
    private readonly CommunicationContext _context = context;

    public async Task DeleteAsync(int id, CancellationToken cancelationToken)
    {
        await _context.Set<UserPost>()
            .Where(cl => cl.Id == id)
            .ExecuteDeleteAsync(cancelationToken);
    }
}
