namespace Communication.Domain.Data;

public interface ICommunityRepository
{
    Task DeleteAsync(int id, CancellationToken cancelationToken);

    Task<int> CountAsync(CancellationToken cancellationToken);
}
