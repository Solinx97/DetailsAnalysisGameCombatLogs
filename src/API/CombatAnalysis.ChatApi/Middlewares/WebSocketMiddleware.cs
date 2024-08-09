using CombatAnalysis.ChatApi.Core;
using System.Net.WebSockets;
using System.Text;

namespace CombatAnalysis.ChatApi.Middlewares;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;

    public WebSocketMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/ws")
        {
            var userId = context.Request.Query["userId"].ToString();
            if (string.IsNullOrEmpty(userId))
            {
                context.Response.StatusCode = 400;
                return;
            }

            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                WebSocketConnectionManager.AddUser(userId, webSocket);

                await HandleStreamWebSocketAsync(context, webSocket, userId);
                WebSocketConnectionManager.RemoveUser(userId);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }
        else
        {
            await _next.Invoke(context);
        }
    }

    private static async Task HandleStreamWebSocketAsync(HttpContext context, WebSocket webSocket, string userId)
    {
        var buffer = new byte[1024 * 4];
        WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        var messageBuilder = new StringBuilder();

        while (!result.CloseStatus.HasValue)
        {
            if (result.MessageType == WebSocketMessageType.Text)
            {
                messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

                if (result.EndOfMessage)
                {
                    var message = messageBuilder.ToString();

                    await HandleCommandAsync(message, userId);

                    messageBuilder.Clear();
                }
            }
            else if (result.MessageType == WebSocketMessageType.Binary)
            {
                await WebSocketConnectionManager.BroadcastBinaryAsync(buffer, result, userId);
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            }

            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        WebSocketConnectionManager.RemoveUser(userId);
    }

    private static async Task HandleCommandAsync(string message, string userId)
    {
        var parts = message.Split(';');
        var command = parts[0];

        switch (command)
        {
            case "REQUEST_MIC_STATUS":
                await WebSocketConnectionManager.BroadcastMessageAsync("requestedMicStatus", userId);
                break;
            case "MIC_STATUS":
                var isOn = parts[1] == "on";
                await WebSocketConnectionManager.BroadcastMessageAsync($"microphoneStatus;{userId};{isOn}", userId);
                break;
            case "JOINED":
                await WebSocketConnectionManager.BroadcastMessageAsync($"joined;{userId};User {userId} has joined the chat", userId);
                break;
            case "LEAVED":
                await WebSocketConnectionManager.BroadcastMessageAsync($"leaved;{userId};User {userId} leaved from chat", userId);
                break;
            default:
                // Handle unknown commands if necessary
                break;
        }
    }

    private static async Task NotifyAboutJoinAsync(string userId)
    {
        var notificationMessage = $"joined;{userId};User {userId} has joined the chat.";
        await WebSocketConnectionManager.BroadcastMessageAsync(notificationMessage, userId);
    }
}
