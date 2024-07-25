namespace CombatAnalysis.CommunicationAPI.Models.Community;

public class CommunityModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int PolicyType { get; set; }

    public string AppUserId { get; set; }
}
