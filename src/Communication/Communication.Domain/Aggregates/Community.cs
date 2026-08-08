using Communication.Domain.Entities.Community;

namespace Communication.Domain.Aggregates;

public class Community
{
    public const int NAME_MAX_LENGTH = 128;

    private readonly List<CommunityPost> _communityPosts = [];
    private readonly List<CommunityDiscussion> _communityDiscussions = [];
    private readonly List<CommunityUser> _communityUsers = [];
    private readonly List<InviteToCommunity> _invitesToCommunity = [];

    private Community()
    {
    }

    private Community(string name, string description, int policyType, string appUserId)
    {
        Name = name;
        Description = description;
        PolicyType = policyType;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public int PolicyType { get; private set; }

    public string AppUserId { get; private set; }

    public IReadOnlyList<CommunityPost> CommunityPosts => _communityPosts;

    public IReadOnlyList<CommunityDiscussion> CommunityDiscussions => _communityDiscussions;

    public IReadOnlyList<CommunityUser> CommunityUsers => _communityUsers;

    public IReadOnlyList<InviteToCommunity> InvitesToCommunity => _invitesToCommunity;

    public static Community Create(string name, string description, int policyType, string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(description, nameof(description));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var community = new Community(name, description, policyType, appUserId);
        community.AddMember(appUserId);

        return community;
    }

    public void AddMember(string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var communityUser = CommunityUser.Create(appUserId);
        _communityUsers.Add(communityUser);
    }

    public void EditName(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

        if (!string.Equals(Name, name, StringComparison.CurrentCultureIgnoreCase))
        {
            Name = name;
        }
    }

    public void EditDescription(string description)
    {
        ArgumentException.ThrowIfNullOrEmpty(description, nameof(description));

        if (!string.Equals(Description, description, StringComparison.CurrentCultureIgnoreCase))
        {
            Description = description;
        }
    }

    public void SetPolicyType(int policyType)
    {
        if (PolicyType != policyType)
        {
            PolicyType = policyType;
        }
    }
}
