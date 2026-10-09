using Chat.Domain.Aggregates;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Repositories;

public interface IPersonalChatRepository
{
    Task AddAsync(PersonalChat chat, CancellationToken cancelationToken);

    Task<IEnumerable<PersonalChat>> GetByUserIdAsync(UserId userId, CancellationToken cancelationToken);

    Task<bool> IsExistAsync(UserId initiatorId, UserId companionId, CancellationToken cancelationToken);
}
