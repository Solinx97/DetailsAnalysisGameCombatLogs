using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Patches;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients;

internal class GroupChatApiClient(HttpClient httpClient) : IGroupChatApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<int> CountAsync(int chatId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"GroupChatMessage/count/{chatId}", cancellationToken);

        var count = await response.Content.ReadFromJsonAsync<int>(cancellationToken);
        return count;
    }

    public async Task<IEnumerable<GroupChatMessageModel>> GetByChatIdAsync(int chatId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"GroupChatMessage/getByChatId?chatId={chatId}&page={page}&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageModel>>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Chat API returned an empty response.");
    }

    public async Task<GroupChatMessageModel> CreateAsync(GroupChatMessageModel message, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsync("PersonalChatMessage", JsonContent.Create(message), cancellationToken);

        var createdMessage = await response.Content.ReadFromJsonAsync<GroupChatMessageModel>(cancellationToken);
        return createdMessage ?? throw new InvalidOperationException("The Chat API returned an empty response.");
    }

    public async Task PatchAsync(string id, GroupChatMessagePatch message, CancellationToken cancellationToken)
    {
        if (id != message.Id)
        {
            throw new InvalidOperationException("Id is not equal message id.");
        }

        await _httpClient.PatchAsync($"GroupChatMessage/{id}", JsonContent.Create(message), cancellationToken);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"GroupChatMessage/{id}", cancellationToken);
    }
}
