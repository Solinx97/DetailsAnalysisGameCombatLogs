using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Models.Community;

public record InviteToCommunityModel(
    [Range(0, int.MaxValue)] int Id,
    [Range(0, int.MaxValue)] int CommunityId,
    [Required] string ToAppUserId,
    [Required] DateTimeOffset CreatedAt,
    [Required] string AppUserId
    );
