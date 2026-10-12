using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Models.Notification;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients;

internal class NotificationApiClient(HttpClient httpClient) : INotificationApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task CreateAsync(CreateGroupChatModel chat, CancellationToken cancellationToken)
    {
        await _httpClient.PostAsync("Notification", JsonContent.Create(chat), cancellationToken);
    }

    public async Task<IEnumerable<NotificationModel>> GetByRecipientIdAsync(Guid recipientId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"Notification/getByRecipientId/{recipientId}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<IEnumerable<NotificationModel>>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Notification API returned an empty response.");
    }

    public async Task<IEnumerable<NotificationModel>> GetUnreadByRecipientIdAsync(Guid recipientId, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"Notification/getUnreadByRecipientId/{recipientId}", cancellationToken);

        var messages = await response.Content.ReadFromJsonAsync<IEnumerable<NotificationModel>>(cancellationToken);
        return messages ?? throw new InvalidOperationException("The Notification API returned an empty response.");
    }

    public async Task DeleteNotificationAsync(int id, CancellationToken cancellationToken)
    {
        await _httpClient.DeleteAsync($"Notification/{id}", cancellationToken);
    }
}
