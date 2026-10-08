using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Patches;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients;

internal class PersonalChatApiClient(HttpClient httpClient) : IPersonalChatApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task CreateAsync(PersonalChatModel chat, CancellationToken cancellationToken)
    {
        await _httpClient.PostAsync("PersonalChat", JsonContent.Create(chat), cancellationToken);
    }

    public async Task<PersonalChatModel> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"PersonalChat/{id}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<PersonalChatModel>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Chat API returned an empty response.");
    }

    public async Task<IEnumerable<PersonalChatModel>> GetByUserIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"PersonalChat/getByUserId/{id}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<IEnumerable<PersonalChatModel>>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Chat API returned an empty response.");
    }

    public async Task<bool> IsChatExistAsync(Guid initiatorId, Guid companionId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"PersonalChat/isExist?initiatorId={initiatorId}&companionId={companionId}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<bool>(cancellationToken);
        return messages;
    }

    public async Task DeleteChatAsync(int id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"PersonalChat/{id}", cancellationToken);
    }

    public async Task<int> CountMessagesAsync(int chatId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"PersonalChatMessage/count/{chatId}", cancellationToken);

        var count = await response.Content.ReadFromJsonAsync<int>(cancellationToken);
        return count;
    }

    public async Task<IEnumerable<PersonalChatMessageModel>> GetMessagesByChatIdAsync(int chatId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"PersonalChatMessage/getByChatId/{chatId}?page={page}&pageSize={pageSize}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<IEnumerable<PersonalChatMessageModel>>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Chat API returned an empty response.");
    }

    public async Task CreateMessageAsync(PersonalChatMessageModel message, CancellationToken cancellationToken)
    {
        await _httpClient.PostAsync("PersonalChatMessage", JsonContent.Create(message), cancellationToken);
    }

    public async Task UpdateMessageAsync(Guid id, PersonalChatMessagePatch message, CancellationToken cancellationToken)
    {
        if (id != message.Id)
        {
            throw new InvalidOperationException("Id is not equal message id.");
        }

        await _httpClient.PatchAsync($"PersonalChatMessage/{id}", JsonContent.Create(message), cancellationToken);
    }

    public async Task DeleteMessageAsync(Guid id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"PersonalChatMessage/{id}", cancellationToken);
    }
}
