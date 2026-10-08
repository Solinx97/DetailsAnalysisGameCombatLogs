using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Patches;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IPersonalChatApiClient
{
    Task CreateAsync(PersonalChatModel chat, CancellationToken cancellationToken);

    Task<PersonalChatModel> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<PersonalChatModel>> GetByUserIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> IsChatExistAsync(Guid initiatorId, Guid companionId, CancellationToken cancellationToken);

    Task DeleteChatAsync(int id, CancellationToken cancellationToken);

    Task<int> CountMessagesAsync(int chatId, CancellationToken cancellationToken);

    Task<IEnumerable<PersonalChatMessageModel>> GetMessagesByChatIdAsync(int chatId, int page, int pageSize, CancellationToken cancellationToken);

    Task CreateMessageAsync(PersonalChatMessageModel message, CancellationToken cancellationToken);

    Task UpdateMessageAsync(Guid id, PersonalChatMessagePatch message, CancellationToken cancellationToken);

    Task DeleteMessageAsync(Guid id, CancellationToken cancellationToken);
}
