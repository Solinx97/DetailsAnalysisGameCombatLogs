using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Patches;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IPersonalChatApiClient
{
    Task<int> CountAsync(int chatId, CancellationToken cancellationToken);

    Task<IEnumerable<PersonalChatMessageModel>> GetByChatIdAsync(int chatId, int page, int pageSize, CancellationToken cancellationToken);

    Task CreateAsync(PersonalChatMessageModel message, CancellationToken cancellationToken);

    Task PatchAsync(string id, PersonalChatMessagePatch message, CancellationToken cancellationToken);

    Task DeleteAsync(string id, CancellationToken cancellationToken);
}
