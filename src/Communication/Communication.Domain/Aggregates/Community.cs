using Communication.Domain.Entities.Community;
using Communication.Domain.Exceptions;
using Communication.Domain.ValueObjects;

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

    private Community(string name, string description, int policyType, UserId appUserId)
    {
        Name = name;
        Description = description;
        PolicyType = policyType;
        AppUserId = appUserId;
    }

    public CommunityId Id { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public int PolicyType { get; private set; }

    public UserId AppUserId { get; private set; }

    public IReadOnlyList<CommunityPost> CommunityPosts => _communityPosts;

    public IReadOnlyList<CommunityDiscussion> CommunityDiscussions => _communityDiscussions;

    public IReadOnlyList<CommunityUser> CommunityUsers => _communityUsers;

    public IReadOnlyList<InviteToCommunity> InvitesToCommunity => _invitesToCommunity;

    public static Community Create(string name, string description, int policyType, UserId appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(description, nameof(description));

        var community = new Community(name, description, policyType, appUserId);
        community.AddMember(appUserId);

        return community;
    }

    public void AddMember(UserId appUserId)
    {
        var communityUser = CommunityUser.Create(appUserId);
        _communityUsers.Add(communityUser);
    }

    public void LeaveCommunity(UserId appUserId)
    {
        var member = _communityUsers
            .FirstOrDefault(x => x.AppUserId == appUserId)
                ?? throw new DomainException($"Community user not found with id {appUserId}");

        _communityUsers.Remove(member);
    }

    public void RemoveMember(CommunityUserId communityUserId)
    {
        var member = _communityUsers
            .FirstOrDefault(x => x.Id == communityUserId)
                ?? throw new DomainException($"Community user not found with id {communityUserId}");

        _communityUsers.Remove(member);
    }

    public void CreateInvite(UserId appUserId, UserId toAppUserId)
    {
        if (IsExist(toAppUserId) || InviteAlreadySent(toAppUserId))
        {
            return;
        }

        var invite = InviteToCommunity.Create(toAppUserId, appUserId);
        _invitesToCommunity.Add(invite);
    }

    public void AcceptInvite(InviteToCommunityId inviteId, UserId appUserId)
    {
        AddMember(appUserId);
        RemoveInvite(inviteId);
    }

    public void RemoveInvite(InviteToCommunityId inviteId)
    {
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

    private bool IsExist(UserId appUserId)
    {
        var user = _communityUsers
            .FirstOrDefault(x => x.AppUserId == appUserId);
        return user != null;
    }

    private bool InviteAlreadySent(UserId toAppUserId)
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
