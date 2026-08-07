using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Models.Community;

public record CommunityModel(
    [Range(0, int.MaxValue)] int Id,
    [Required] string Name,
    [Required] string Description,
    [Range(0, int.MaxValue)] int PolicyType,
    [Required] string AppUserId
    );
