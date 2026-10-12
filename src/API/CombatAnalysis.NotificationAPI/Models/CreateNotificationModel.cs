using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.NotificationAPI.Models;

public record CreateNotificationModel(
    [Required] int Type,
    [Required] Guid InitiatorId,
    string? InitiatorName,
    [Required] Guid RecipientId
    );
