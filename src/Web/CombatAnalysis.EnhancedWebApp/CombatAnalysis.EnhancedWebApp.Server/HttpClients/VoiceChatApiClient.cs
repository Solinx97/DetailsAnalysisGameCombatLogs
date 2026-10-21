using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients;

public class VoiceChatApiClient(HttpClient httpClient) : IVoiceChatApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task CreateAsync(CreateVoiceChatModel chat, CancellationToken cancellationToken)
    {
        await _httpClient.PostAsync("VoiceChat", JsonContent.Create(chat), cancellationToken);
    }

    public async Task<VoiceChatModel> GetByChatIdAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"VoiceChat/getByChatId/{id}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<VoiceChatModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Chat API returned an empty response.");
    }

    public async Task<bool> IsChatExistAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"VoiceChat/isChatExist/{id}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<bool>(cancellationToken);
        return messages;
    }

    public async Task<VoiceChatTokenResponse> GetTokenAsync(int voiceChatId, GetVoiceChatTokenRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync($"VoiceChat/token/{voiceChatId}", JsonContent.Create(request), cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<VoiceChatTokenResponse>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Chat API returned an empty response.");
    }

    public async Task DeleteChatAsync(Guid id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"VoiceChat/{id}", cancellationToken);
    }
}
