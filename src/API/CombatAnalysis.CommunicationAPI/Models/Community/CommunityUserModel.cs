using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Models.Community;

public record CommunityUserModel(
    [Required] string Id,
    [Range(0, int.MaxValue)] int CommunityId,
    [Required] string AppUserId
    );
