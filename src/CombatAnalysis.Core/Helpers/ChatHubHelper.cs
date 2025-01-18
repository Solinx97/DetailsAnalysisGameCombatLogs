using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CombatAnalysis.Core.Helpers;

internal class ChatHubHelper : IChatHubHelper
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger _logger;

    private HubConnection? _chatHubConnection;
    private HubConnection? _chatMessagesCountHubConnection;
     
    public ChatHubHelper(IMemoryCache memoryCache, ILogger logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task ConnectToChatHubAsync(string hubURL)
    {
        try
        {
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            var accessToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.AccessToken));

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }
            else if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            _chatHubConnection = await CreateHubConnectionAsync(hubURL, refreshToken, accessToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task JoinChatRoom(int chatId)
    {
        if (_chatHubConnection == null)
        {
            return;
        }

        await _chatHubConnection.SendAsync("JoinRoom", chatId);
    }

    public async Task SendMessageAsync(string message, int chatId, string appUserId, string username)
    {
        if (_chatHubConnection == null)
        {
            return;
        }

        await _chatHubConnection.SendAsync("SendMessage", message, chatId, appUserId, username);
    }

    public async Task ConnectToChatMessageCountHubAsync(string hubURL)
    {
        try
        {
            var refreshToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.RefreshToken));
            var accessToken = _memoryCache.Get<string>(nameof(MemoryCacheValue.AccessToken));

            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }
            else if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            _chatMessagesCountHubConnection = await CreateHubConnectionAsync(hubURL, refreshToken, accessToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task JoinChatMessageCountRoom(int chatId)
    {
        if (_chatMessagesCountHubConnection == null)
        {
            return;
        }

        await _chatMessagesCountHubConnection.SendAsync("JoinRoom", chatId);
    }

    public void SubscribeMessageCountUpdated(string meInChatId, Action<int, int> receiveUnreadMessageCountAction)
    {
        if (_chatMessagesCountHubConnection == null)
        {
            return;
        }

        _chatMessagesCountHubConnection.On<int>("ReceiveUnreadMessageIncreased", async (chatId) => 
        {
            await _chatMessagesCountHubConnection.SendAsync("RequestUnreadMessages", chatId, meInChatId);
        });

        _chatMessagesCountHubConnection.On("ReceiveUnreadMessageCount", receiveUnreadMessageCountAction);
    }

    public void SubscribeMessagesUpdated<T>(int chatId, string meInChatId, Action<T> action)
        where T : class
    {
        if (_chatHubConnection == null || _chatMessagesCountHubConnection == null)
        {
            return;
        }

        _chatHubConnection.On("ReceiveMessageHasBeenRead", async () =>
        {
            await _chatMessagesCountHubConnection.SendAsync("RequestUnreadMessages", chatId, meInChatId);
        });

        _chatHubConnection.On("ReceiveMessage", action);

        _chatHubConnection.On("MessageDelivered", async () => {
            await _chatMessagesCountHubConnection.SendAsync("SendUnreadMessageIncreased", chatId);
        });
    }

    public async Task SubscribeMessageHasBeenReadAsync(int messageId, string appUserId)
    {
        if (_chatHubConnection == null)
        {
            return;
        }

        await _chatHubConnection.SendAsync("SendMessageHasBeenRead", messageId, appUserId);
    }

    private static async Task<HubConnection> CreateHubConnectionAsync(string hubURL, string refreshToken, string accessToken)
    {
        var cookieContainer = new CookieContainer();
        cookieContainer.Add(new Uri(hubURL), new Cookie(nameof(MemoryCacheValue.RefreshToken), refreshToken));
        cookieContainer.Add(new Uri(hubURL), new Cookie(nameof(MemoryCacheValue.AccessToken), accessToken));

        var hub = new HubConnectionBuilder()
            .WithUrl(hubURL, options =>
            {
                options.Cookies = cookieContainer;
            })
            .Build();

        await hub.StartAsync();

        return hub;
    }
}
