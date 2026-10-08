using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Patches;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients;

internal class PersonalChatApiClient(HttpClient httpClient) : IPersonalChatApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<int> CountAsync(int chatId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"PersonalChatMessage/count/{chatId}", cancellationToken);

        var count = await response.Content.ReadFromJsonAsync<int>(cancellationToken);
        return count;
    }

    public async Task<IEnumerable<PersonalChatMessageModel>> GetByChatIdAsync(int chatId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"PersonalChatMessage/getByChatId/{chatId}?page={page}&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Chat API returned an empty response.");
    }

    public async Task CreateAsync(PersonalChatMessageModel message, CancellationToken cancellationToken)
    {
        await _httpClient.PostAsync("PersonalChatMessage", JsonContent.Create(message), cancellationToken);
    }

    public async Task PatchAsync(string id, PersonalChatMessagePatch message, CancellationToken cancellationToken)
    {
        if (id != message.Id)
        {
            throw new InvalidOperationException("Id is not equal message id.");
        }

        await _httpClient.PatchAsync($"PersonalChatMessage/{id}", JsonContent.Create(message), cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"PersonalChatMessage/{id}", cancellationToken);
    }
}
