using Chat.Domain.Aggregates;

namespace Chat.Domain.Repositories;

public interface IPersonalChatRepository
{
    Task AddAsync(PersonalChat chat, CancellationToken cancelationToken);

    Task<IEnumerable<PersonalChat>> GetByUserIdAsync(Guid userId, CancellationToken cancelationToken);

    Task<bool> IsExistAsync(Guid initiatorId, Guid companionId, CancellationToken cancelationToken);
}
