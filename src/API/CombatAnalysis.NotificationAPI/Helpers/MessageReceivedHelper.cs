using CombatAnalysis.NotificationAPI.Consts;

namespace CombatAnalysis.NotificationAPI.Helpers;

internal static class MessageReceivedHelper
{
    public static bool IsHubExist(PathString pathString)
    {
        var isExist = pathString.StartsWithSegments(HubPatterns.NOTIFICATION);

        return isExist;
    }
}
