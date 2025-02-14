﻿using CombatAnalysis.Hubs.Enums;
using CombatAnalysis.Hubs.Interfaces;
using CombatAnalysis.Hubs.Models;
using Microsoft.AspNetCore.SignalR;

namespace CombatAnalysis.Hubs.Hubs;

public class PersonalChatMessagesHub : Hub
{
    private readonly IHttpClientHelper _httpClient;
    private readonly ILogger<PersonalChatMessagesHub> _logger;

    public PersonalChatMessagesHub(IHttpClientHelper httpClient, ILogger<PersonalChatMessagesHub> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task JoinRoom(int chatId)
    {
        try
        {
            var context = Context.GetHttpContext();
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.RefreshToken), out var refreshToken))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            }
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

    public async Task SendMessage(string message, int chatId, string appUserId, string username)
    {
        try
        {
            var personalMessage = new PersonalChatMessageModel
            {
                Username = username,
                Message = message,
                Time = TimeSpan.Parse($"{DateTimeOffset.UtcNow.Hour}:{DateTimeOffset.UtcNow.Minute}").ToString(),
                Status = 0,
                ChatId = chatId,
                AppUserId = appUserId
            };

            var response = await _httpClient.PostAsync("PersonalChatMessage", JsonContent.Create(personalMessage));
            response.EnsureSuccessStatusCode();

            var createdMessage = await response.Content.ReadFromJsonAsync<PersonalChatMessageModel>();
            if (createdMessage == null)
            {
                throw new ArgumentNullException(nameof(createdMessage));
            }

            await Clients.Caller.SendAsync("ReceiveMessageDelivered");

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", createdMessage);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task SendMessageHasBeenRead(int chatMessageId, string meId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"PersonalChatMessage/{chatMessageId}");
            response.EnsureSuccessStatusCode();

            var messageModel = await response.Content.ReadFromJsonAsync<PersonalChatMessageModel>();
            if (messageModel == null)
            {
                throw new ArgumentNullException(nameof(messageModel));
            }

            if (messageModel.Status == 2)
            {
                return;
            }

            messageModel.Status = 2;

            response = await _httpClient.PutAsync("PersonalChatMessage", JsonContent.Create(messageModel));
            response.EnsureSuccessStatusCode();

            response = await _httpClient.GetAsync($"PersonalChatMessageCount/findMe?chatId={messageModel.ChatId}&appUserId={meId}");
            response.EnsureSuccessStatusCode();

            var messageCount = await response.Content.ReadFromJsonAsync<PersonalChatMessageCountModel>();
            if (messageCount == null)
            {
                throw new ArgumentNullException(nameof(messageCount));
            }

            messageCount.Count--;

            response = await _httpClient.PutAsync("PersonalChatMessageCount", JsonContent.Create(messageCount));
            response.EnsureSuccessStatusCode();

            await Clients.Caller.SendAsync("ReceiveMessageHasBeenRead");
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task LeaveFromRoom(int room)
    {
        var refreshToken = Context.GetHttpContext()?.Request.Cookies[nameof(AuthenticationCookie.RefreshToken)] ?? string.Empty;
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.ToString());
        }
    }
}
