namespace CombatAnalysis.DAL.Entities.Community;

public class Community
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int CommunityPolicyType { get; set; }

    public string OwnerId { get; set; }
}
