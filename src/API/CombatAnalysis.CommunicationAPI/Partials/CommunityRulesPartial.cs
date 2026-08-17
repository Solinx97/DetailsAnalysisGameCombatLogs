using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CommunicationAPI.Partials;

public record CommunityRulesPartial(
    [Range(0, int.MaxValue)] int Id,
    [Range(0, int.MaxValue)] int PolicyType
);
