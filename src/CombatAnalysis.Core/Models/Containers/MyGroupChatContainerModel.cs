using CombatAnalysis.Core.Models.Chat;

namespace CombatAnalysis.Core.Models.Containers;

public class MyGroupChatContainerModel
{
    public GroupChatModel GroupChat { get; set; }

    public GroupChatMessageCountModel GroupChatMessageCount { get; set; }
}
