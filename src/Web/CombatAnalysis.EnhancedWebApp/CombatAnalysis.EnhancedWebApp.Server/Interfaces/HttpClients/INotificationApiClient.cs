using CombatAnalysis.EnhancedWebApp.Server.Models.Chat;
using CombatAnalysis.EnhancedWebApp.Server.Models.Notification;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;

public interface INotificationApiClient
{
    Task CreateAsync(CreateGroupChatModel chat, CancellationToken cancellationToken);

    Task<IEnumerable<NotificationModel>> GetByRecipientIdAsync(Guid recipientId, CancellationToken cancellationToken);

    Task<IEnumerable<NotificationModel>> GetUnreadByRecipientIdAsync(Guid recipientId, CancellationToken cancellationToken);

    Task DeleteNotificationAsync(int id, CancellationToken cancellationToken);
}
