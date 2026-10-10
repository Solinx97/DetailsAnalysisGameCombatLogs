using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IVoiceChatApiClient
{
    Task CreateAsync(CreateVoiceChatModel chat, CancellationToken cancellationToken);

    Task<VoiceChatModel> GetByChatIdAsync(int id, CancellationToken cancellationToken);

    Task<bool> IsChatExistAsync(int id, CancellationToken cancellationToken);

    Task DeleteChatAsync(Guid id, CancellationToken cancellationToken);
}
