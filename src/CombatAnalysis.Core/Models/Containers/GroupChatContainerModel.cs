using CombatAnalysis.Core.Models.Chat;

namespace CombatAnalysis.Core.Models.Containers;

public class GroupChatContainerModel
{
    public GroupChatModel GroupChat { get; set; }

    public GroupChatRulesModel GroupChatRules { get; set; }

    public GroupChatUserModel GroupChatUser { get; set; }
}
