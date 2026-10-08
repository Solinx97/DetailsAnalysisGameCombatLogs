using CombatAnalysis.Hubs.Consts;

namespace CombatAnalysis.Hubs.Helpers;

internal static class MessageReceivedHelper
{
    public static bool IsHubExist(PathString pathString)
    {
        var isExist = pathString.StartsWithSegments(HubPatterns.PersonalChat)
            || pathString.StartsWithSegments(HubPatterns.PersonalChatMessages)
            || pathString.StartsWithSegments(HubPatterns.PersonalChatUnreadMessage)
            || pathString.StartsWithSegments(HubPatterns.GroupChat)
            || pathString.StartsWithSegments(HubPatterns.GroupChatMessages)
            || pathString.StartsWithSegments(HubPatterns.GroupChatUnreadMessage)
            || pathString.StartsWithSegments(HubPatterns.VoiceChat)
            || pathString.StartsWithSegments(HubPatterns.Notification);

        return isExist;
    }
}
