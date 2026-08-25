using Communication.Domain.Entities.Community;
using Communication.Domain.Exceptions;

namespace Communication.Domain.Aggregates;

public class Community
{
    public const int NAME_MAX_LENGTH = 128;
    public const int DESCRIPTION_MAX_LENGTH = 512;

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

    public void LeaveCommunity(string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var member = _communityUsers
            .FirstOrDefault(x => x.AppUserId == appUserId)
                ?? throw new DomainException($"Community user not found with id {appUserId}");

        _communityUsers.Remove(member);
    }

    public void RemoveMember(string communityUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(communityUserId, nameof(communityUserId));

        var member = _communityUsers
            .FirstOrDefault(x => x.Id == communityUserId)
                ?? throw new DomainException($"Community user not found with id {communityUserId}");

        _communityUsers.Remove(member);
    }

    public void CreateInvite(string appUserId, string toAppUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));
        ArgumentException.ThrowIfNullOrEmpty(toAppUserId, nameof(toAppUserId));

        if (IsExist(toAppUserId) || InviteAlreadySent(toAppUserId))
        {
            return;
        }

        var createdAt = DateTimeOffset.UtcNow;
        var invite = InviteToCommunity.Create(toAppUserId, createdAt, appUserId);
        _invitesToCommunity.Add(invite);
    }

    public void AcceptInvite(int inviteId, string appUserId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(inviteId, nameof(inviteId));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        AddMember(appUserId);
        RemoveInvite(inviteId);
    }

    public void RemoveInvite(int inviteId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(inviteId, nameof(inviteId));

        var invite = _invitesToCommunity
            .FirstOrDefault(x => x.Id == inviteId)
                ?? throw new DomainException($"Invite to community not found with id {inviteId}");

        _invitesToCommunity.Remove(invite);
    }

    public void Edit(string name, string description)
    {
        EditName(name);
        EditDescription(description);
    }

    public void SetPolicyType(int policyType)
    {
        if (PolicyType != policyType)
        {
            PolicyType = policyType;
        }
    }

    private bool IsExist(string appUserId)
    {
        var user = _communityUsers
            .FirstOrDefault(x => x.AppUserId == appUserId);
        return user != null;
    }

    private bool InviteAlreadySent(string toAppUserId)
    {
        var invite = _invitesToCommunity
            .FirstOrDefault(x => x.ToAppUserId == toAppUserId);
        return invite != null;
    }

    private void EditName(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

        if (!string.Equals(Name, name, StringComparison.CurrentCultureIgnoreCase))
        {
            Name = name;
        }
    }

    private void EditDescription(string description)
    {
        ArgumentException.ThrowIfNullOrEmpty(description, nameof(description));

        if (!string.Equals(Description, description, StringComparison.CurrentCultureIgnoreCase))
        {
            Description = description;
        }
    }
}
