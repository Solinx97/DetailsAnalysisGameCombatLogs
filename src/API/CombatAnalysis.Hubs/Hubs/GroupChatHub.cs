using Chat.Application.Consts;
using Chat.Application.DTOs;
using Chat.Application.Enums;
using Chat.Application.Kafka.Actions;
using Chat.Application.Security;
using CombatAnalysis.Hubs.Consts;
using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Models;
using CombatAnalysis.Hubs.Models.Containers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.Hubs.Hubs;

[Authorize]
public class GroupChatHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<GroupChatHub> _logger;
    private readonly IKafkaProducerService<string, string> _kafkaProducer;
    private readonly KafkaSettings _kafkaSettings;

    public GroupChatHub(IHttpClientHelper httpClient, IOptions<Cluster> cluster, ILogger<GroupChatHub> logger, 
        IKafkaProducerService<string, string> kafkaProducer, IOptions<KafkaSettings> kafkaSettings)
    {
        _logger = logger;
        _kafkaProducer = kafkaProducer;
        _httpClient = httpClient;
        _httpClient.APIUrl = cluster.Value.Chat;
        _kafkaSettings = kafkaSettings.Value;
    }

    public async Task JoinRoom(string appUserId)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

            await Groups.AddToGroupAsync(Context.ConnectionId, appUserId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Join user to room failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task CreateGroupChat(GroupChatContainerModel container)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(container, nameof(container));

            var encryptedAccessToken = string.Empty;
            var accessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)];

            if (!string.IsNullOrEmpty(accessToken))
            {
                encryptedAccessToken = AesEncryption.Encrypt(accessToken, Convert.FromBase64String(_kafkaSettings.Security.SecurityKey), Convert.FromBase64String(_kafkaSettings.Security.IV));
            }

            var chatAction = JsonSerializer.Serialize(new GroupChatAction
            {
                Chat = container.GroupChat,
                Rules = container.GroupChatRules,
                User = container.GroupChatUser,
                State = ChatActionState.Created,
                When = DateTimeOffset.UtcNow,
                AccessToken = encryptedAccessToken
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChat, Guid.NewGuid().ToString(), chatAction);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task RequestJoinedUser(int chatId, string appUserId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(chatId, 1, nameof(chatId));

            ArgumentNullException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

            var response = await _httpClient.GetAsync($"GroupChatUser/findByAppUserId?chatId={chatId}&appUserId={appUserId}");
            response.EnsureSuccessStatusCode();

            var groupChatUser = await response.Content.ReadFromJsonAsync<GroupChatUserModel>();
            ArgumentNullException.ThrowIfNull(groupChatUser, nameof(groupChatUser));

            await Clients.Group(appUserId).SendAsync("ReceiveJoinedUser", groupChatUser);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Request joined users failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied: user should be authorized.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Request unsuccessful. Status code: '{StatusCode}'", ex.StatusCode);
        }
    }

    public async Task RequestMembers(int chatId, string appUserId)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(chatId, 1, nameof(chatId));

            ArgumentNullException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

            var response = await _httpClient.GetAsync($"GroupChatUser/findAll/{chatId}");
            response.EnsureSuccessStatusCode();

            var groupChatUsers = await response.Content.ReadFromJsonAsync<IEnumerable<GroupChatUserModel>>();
            ArgumentNullException.ThrowIfNull(groupChatUsers, nameof(groupChatUsers));

            await Clients.Group(appUserId).SendAsync("ReceiveMembers", groupChatUsers);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Request joined users failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Access denied: user should be authorized.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Request unsuccessful. Status code: '{StatusCode}'", ex.StatusCode);
        }
    }

    public async Task AddUserToChat(string chatOwnerId, GroupChatUserDto groupChatUser)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(chatOwnerId, nameof(chatOwnerId));
            ArgumentNullException.ThrowIfNull(groupChatUser, nameof(groupChatUser));

            var encryptedAccessToken = string.Empty;
            var accessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)];

            if (!string.IsNullOrEmpty(accessToken))
            {
                encryptedAccessToken = AesEncryption.Encrypt(accessToken, Convert.FromBase64String(_kafkaSettings.Security.SecurityKey), Convert.FromBase64String(_kafkaSettings.Security.IV));
            }

            var chatAction = JsonSerializer.Serialize(new GroupChatMemberAction
            {
                User = groupChatUser,
                ChatOwnerId = chatOwnerId,
                State = ChatMembersActionState.AddUser,
                When = DateTimeOffset.UtcNow,
                AccessToken = encryptedAccessToken
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChatMember, Guid.NewGuid().ToString(), chatAction);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Add user to chat failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task RemoveUserFromChat(string chatOwnerId, int chatId, string groupChatUserId, string groupChatUsername)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(chatId, 1, nameof(chatId));
            ArgumentNullException.ThrowIfNullOrEmpty(groupChatUserId, nameof(groupChatUserId));
            ArgumentNullException.ThrowIfNullOrEmpty(groupChatUsername, nameof(groupChatUsername));

            var chatAction = JsonSerializer.Serialize(new GroupChatMemberAction
            {
                ChatOwnerId = chatOwnerId,
                User = new GroupChatUserDto {
                    Id = groupChatUserId,
                    Username= groupChatUsername,
                    GroupChatId = chatId,
                },
                State = ChatMembersActionState.RemoveUser,
                When = DateTime.UtcNow,
                AccessToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.AccessToken)] ?? string.Empty
            });
            await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChatMember, Guid.NewGuid().ToString(), chatAction);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Add user to chat failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public async Task LeaveFromRoom(string appUserId)
    {
        try
        {
            ArgumentNullException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

            var refreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty;
            ArgumentNullException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, appUserId);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Join user to room failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception != null)
        {
            _logger.LogError(exception, exception.Message);
        }

        return base.OnDisconnectedAsync(exception);
    }
}
