using CombatAnalysis.ChatAPI.Consts;

namespace CombatAnalysis.ChatAPI.Helpers;

internal static class MessageReceivedHelper
{
    public static bool IsHubExist(PathString pathString)
    {
        var isExist = pathString.StartsWithSegments(HubPatterns.PERSONAL_CHAT_MESSAGE)
            || pathString.StartsWithSegments(HubPatterns.GROUP_CHAT_MESSAGE)
            || pathString.StartsWithSegments(HubPatterns.GROUP_CHAT)
            || pathString.StartsWithSegments(HubPatterns.PERSONAL_CHAT)
            || pathString.StartsWithSegments(HubPatterns.VOICE_CHAT);

        return isExist;
    }
}
