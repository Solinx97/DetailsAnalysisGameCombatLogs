using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Patches;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IGroupChatApiClient
{
    Task<int> CountAsync(int chatId, CancellationToken cancellationToken);

    Task<IEnumerable<GroupChatMessageModel>> GetByChatIdAsync(int chatId, int page, int pageSize, CancellationToken cancellationToken);

    Task<GroupChatMessageModel> CreateAsync(GroupChatMessageModel message, CancellationToken cancellationToken);

    Task PatchAsync(string id, GroupChatMessagePatch message, CancellationToken cancellationToken);

    Task DeleteAsync(string id, CancellationToken cancellationToken);
}
