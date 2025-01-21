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

    private HubConnection? _chatMessagesHubConnection;
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

            _chatMessagesHubConnection = await CreateHubConnectionAsync(hubURL, refreshToken, accessToken);
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

    public async Task JoinChatRoomAsync(int chatId)
    {
        if (_chatMessagesHubConnection == null)
        {
            return;
        }

        await _chatMessagesHubConnection.SendAsync("JoinRoom", chatId);
    }

    public async Task SendMessageAsync(string message, int chatId, string appUserId, string username, int type = -1)
    {
        if (_chatMessagesHubConnection == null)
        {
            return;
        }

        if (type > -1)
        {
            await _chatMessagesHubConnection.SendAsync("SendMessage", message, chatId, type, appUserId, username);
        }
        else
        {
            await _chatMessagesHubConnection.SendAsync("SendMessage", message, chatId, appUserId, username);
        }
    }

    public async Task ConnectToUnreadMessageHubAsync(string hubURL)
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

    public async Task JoinUnreadMessageRoomAsync(int chatId)
    {
        if (_chatMessagesCountHubConnection == null)
        {
            return;
        }

        await _chatMessagesCountHubConnection.SendAsync("JoinRoom", chatId);
    }

    public void SubscribeUnreadMessagesUpdated(string meInChatId, Action<int, string, int> receiveUnreadMessageAction)
    {
        if (_chatMessagesCountHubConnection == null)
        {
            return;
        }

        _chatMessagesCountHubConnection.On<int>("ReceiveUnreadMessageUpdated", async (chatId) => 
        {
            await _chatMessagesCountHubConnection.SendAsync("RequestUnreadMessages", chatId, meInChatId);
        });

        _chatMessagesCountHubConnection.On("ReceiveUnreadMessage", receiveUnreadMessageAction);
    }

    public void SubscribeMessagesUpdated<T>(int chatId, string meInChatId, Action<T> action)
        where T : class
    {
        if (_chatMessagesHubConnection == null || _chatMessagesCountHubConnection == null)
        {
            return;
        }

        _chatMessagesHubConnection.On("ReceiveMessageHasBeenRead", async () =>
        {
            await _chatMessagesCountHubConnection.SendAsync("RequestUnreadMessages", chatId, meInChatId);
        });

        _chatMessagesHubConnection.On("ReceiveMessage", action);

        _chatMessagesHubConnection.On("ReceiveMessageDelivered", async () => {
            await _chatMessagesCountHubConnection.SendAsync("SendUnreadMessageUpdated", chatId);
        });
    }

    public async Task SubscribeMessageHasBeenReadAsync(int messageId, string appUserId)
    {
        if (_chatMessagesHubConnection == null)
        {
            return;
        }

        await _chatMessagesHubConnection.SendAsync("SendMessageHasBeenRead", messageId, appUserId);
    }

    public async Task LeaveFromChatRoomAsync(int chatId)
    {
        if (_chatMessagesHubConnection == null)
        {
            return;
        }

        await _chatMessagesHubConnection.SendAsync("LeaveFromRoom", chatId);
    }

    public async Task LeaveFromUnreadMessageRoomAsync(int chatId)
    {
        if (_chatMessagesCountHubConnection == null)
        {
            return;
        }

        await _chatMessagesCountHubConnection.SendAsync("LeaveFromRoom", chatId);
    }

    public async Task StopAsync()
    {
        if (_chatMessagesCountHubConnection != null)
        {
            await _chatMessagesCountHubConnection.StopAsync();
        }

        if (_chatMessagesHubConnection != null)
        {
            await _chatMessagesHubConnection.StopAsync();
        }
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
