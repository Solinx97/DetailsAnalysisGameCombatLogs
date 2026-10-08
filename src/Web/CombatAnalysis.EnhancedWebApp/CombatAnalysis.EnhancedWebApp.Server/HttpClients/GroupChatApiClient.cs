using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Patches;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients;

internal class GroupChatApiClient(HttpClient httpClient) : IGroupChatApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task CreateAsync(CreateGroupChatModel chat, CancellationToken cancellationToken)
    {
        await _httpClient.PostAsync("GroupChat", JsonContent.Create(chat), cancellationToken);
    }

    public async Task<GroupChatModel> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"GroupChat/{id}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<GroupChatModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Chat API returned an empty response.");
    }

    public async Task UpdateChatNameAsync(int id, GroupChatPatch chat, CancellationToken cancellationToken)
    {
        if (id != chat.Id)
        {
            throw new InvalidOperationException("Id is not equal message id.");
        }

        await _httpClient.PatchAsync($"GroupChat/{id}", JsonContent.Create(chat), cancellationToken);
    }

    public async Task UpdateRulesAsync(int id, GroupChatRulesModel rules, CancellationToken cancellationToken)
    {
        if (id != rules.Id)
        {
            throw new InvalidOperationException("Id is not equal message id.");
        }

        await _httpClient.PatchAsync($"GroupChat/updateRules/{id}", JsonContent.Create(rules), cancellationToken);
    }

    public async Task<GroupChatRulesModel> GetRulesAsync(int chatId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"GroupChat/getRules/{chatId}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<GroupChatRulesModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Chat API returned an empty response.");
    }

    public async Task DeleteChatAsync(int id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"GroupChat/{id}", cancellationToken);
    }

    public async Task<int> CountMessagesAsync(int chatId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"GroupChatMessage/count/{chatId}", cancellationToken);

        var count = await response.Content.ReadFromJsonAsync<int>(cancellationToken);
        return count;
    }

    public async Task<IEnumerable<GroupChatMessageModel>> GetMessagesByChatIdAsync(int chatId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"GroupChatMessage/getByChatId/{chatId}?page={page}&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatMessageModel>>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Chat API returned an empty response.");
    }

    public async Task CreateMessageAsync(GroupChatMessageModel message, CancellationToken cancellationToken)
    {
        await _httpClient.PostAsync("GroupChatMessage", JsonContent.Create(message), cancellationToken);
    }

    public async Task UpdateMessageAsync(Guid id, GroupChatMessagePatch message, CancellationToken cancellationToken)
    {
        if (id != message.Id)
        {
            throw new InvalidOperationException("Id is not equal message id.");
        }

        await _httpClient.PatchAsync($"GroupChatMessage/{id}", JsonContent.Create(message), cancellationToken);
    }

    public async Task DeleteMessageAsync(Guid id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"GroupChatMessage/{id}", cancellationToken);
    }
}
