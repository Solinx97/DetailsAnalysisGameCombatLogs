using CombatAnalysis.Core.Models.Chat;

namespace CombatAnalysis.Core.Models.Containers;

public class MyPersonalChatContainerModel
{
    public PersonalChatModel PersonalChat { get; set; }

    public PersonalChatMessageCountModel PersonalChatMessageCount { get; set; }
}
