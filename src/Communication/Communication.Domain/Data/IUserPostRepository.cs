namespace Communication.Domain.Data;

public interface IUserPostRepository
{
    Task DeleteAsync(int id, CancellationToken cancelationToken);
}
