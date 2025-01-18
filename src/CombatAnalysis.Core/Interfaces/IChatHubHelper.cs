namespace CombatAnalysis.Core.Interfaces;

public interface IChatHubHelper
{
    Task ConnectToChatHubAsync(string hubURL);

    Task JoinChatRoom(int chatId);

    Task SendMessageAsync(string message, int chatId, string appUserId, string username);

    Task ConnectToChatMessageCountHubAsync(string hubURL);

    Task JoinChatMessageCountRoom(int chatId);

    void SubscribeMessageCountUpdated(string meInChatId, Action<int, int> receiveUnreadMessageCountAction);

    void SubscribeMessagesUpdated<T>(int chatId, string meInChatId, Action<T> action) where T : class;

    Task SubscribeMessageHasBeenReadAsync(int messageId, string appUserId);
}
