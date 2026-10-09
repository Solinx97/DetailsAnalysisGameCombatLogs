using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Patches;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IGroupChatApiClient
{
    Task CreateAsync(CreateGroupChatModel chat, CancellationToken cancellationToken);

    Task<GroupChatModel> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task UpdateChatNameAsync(int id, GroupChatPatch chat, CancellationToken cancellationToken);

    Task UpdateRulesAsync(int id, GroupChatRulesModel rules, CancellationToken cancellationToken);

    Task<GroupChatRulesModel> GetRulesAsync(int chatId, CancellationToken cancellationToken);

    Task DeleteChatAsync(int id, CancellationToken cancellationToken);

    Task CreateMessageAsync(GroupChatMessageModel message, CancellationToken cancellationToken);

    Task<int> CountMessagesAsync(int chatId, CancellationToken cancellationToken);

    Task<IEnumerable<GroupChatMessageModel>> GetMessagesByChatIdAsync(int chatId, int page, int pageSize, CancellationToken cancellationToken);

    Task UpdateMessageAsync(Guid id, GroupChatMessagePatch message, CancellationToken cancellationToken);

    Task DeleteMessageAsync(Guid id, CancellationToken cancellationToken);

    Task AddUserAsync(CreateGroupChatUserModel user, CancellationToken cancellationToken);

    Task<GroupChatUserModel> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<GroupChatUserModel>> FindAllChatUsersAsync(int chatId, CancellationToken cancellationToken);

    Task<GroupChatUserModel> FindChatUserAsync(Guid appUserId, int chatId, CancellationToken cancellationToken);

    Task<IEnumerable<GroupChatUserModel>> FindChatUsersAsync(Guid appUserId, CancellationToken cancellationToken);

    Task LeaveFromChatAsync(Guid id, int chatId, CancellationToken cancellationToken);

    Task DeleteChatUserAsync(Guid id, int chatId, Guid whoDeleteId, CancellationToken cancellationToken);
}
