namespace CombatAnalysis.Core.Interfaces;

public interface IChatHubHelper
{
    Task ConnectToChatHubAsync(string hubURL);

    Task JoinChatRoomAsync(int chatId);

    Task SendMessageAsync(string message, int chatId, string appUserId, string username, int type = -1);

    Task ConnectToUnreadMessageHubAsync(string hubURL);

    Task JoinUnreadMessageRoomAsync(int chatId);

    void SubscribeUnreadMessagesUpdated(string meInChatId, Action<int, string, int> receiveUnreadMessageAction);

    void SubscribeMessagesUpdated<T>(int chatId, string meInChatId, Action<T> action) where T : class;

    Task SubscribeMessageHasBeenReadAsync(int messageId, string appUserId);

    Task LeaveFromChatRoomAsync(int chatId);

    Task LeaveFromUnreadMessageRoomAsync(int chatId);

    Task StopAsync();
}
