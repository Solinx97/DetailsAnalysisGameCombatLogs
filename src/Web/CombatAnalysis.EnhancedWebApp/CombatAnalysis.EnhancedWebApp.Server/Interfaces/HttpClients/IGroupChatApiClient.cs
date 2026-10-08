using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Patches;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IGroupChatApiClient
{
    Task CreateAsync(CreateGroupChatModel message, CancellationToken cancellationToken);

    Task<int> CountMessagesAsync(int chatId, CancellationToken cancellationToken);

    Task<GroupChatModel> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task UpdateChatNameAsync(int id, GroupChatPatch message, CancellationToken cancellationToken);

    Task DeleteChatAsync(int id, CancellationToken cancellationToken);

    Task UpdateRulesAsync(int id, GroupChatRulesModel rules, CancellationToken cancellationToken);

    Task<GroupChatRulesModel> GetRulesAsync(int chatId, CancellationToken cancellationToken);

    Task<IEnumerable<GroupChatMessageModel>> GetMessagesByChatIdAsync(int chatId, int page, int pageSize, CancellationToken cancellationToken);

    Task CreateMessageAsync(GroupChatMessageModel message, CancellationToken cancellationToken);

    Task UpdateMessageAsync(Guid id, GroupChatMessagePatch message, CancellationToken cancellationToken);

    Task DeleteMessageAsync(Guid id, CancellationToken cancellationToken);
}
