using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Partials;

public record CommunityDiscussionPartial(
    [Range(0, int.MaxValue)] int Id,
    string Title,
    string Content
    );
