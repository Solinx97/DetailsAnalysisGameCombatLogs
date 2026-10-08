using CombatAnalysis.ChatAPI.Consts;

namespace CombatAnalysis.ChatAPI.Helpers;

internal static class MessageReceivedHelper
{
    public static bool IsHubExist(PathString pathString)
    {
        var isExist = pathString.StartsWithSegments(HubPatterns.PERSONAL_CHAT_MESSAGE);

        return isExist;
    }
}
