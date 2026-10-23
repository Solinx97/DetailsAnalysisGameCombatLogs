using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface IVoiceChatApiClient
{
    Task CreateAsync(CreateVoiceChatModel chat, CancellationToken cancellationToken);

    Task<bool> IsChatExistAsync(int id, CancellationToken cancellationToken);

    Task<int> GetParticipantsCountAsync(int id, CancellationToken cancellationToken);

    Task<VoiceChatTokenResponse> GetTokenAsync(int voiceChatId, GetVoiceChatTokenRequest request, CancellationToken cancellationToken);

    Task DeleteChatAsync(Guid id, CancellationToken cancellationToken);
}
