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
        if (context.Request.Path != "/ws")
        {
            await _next.Invoke(context);
            return;
        }

        var userId = context.Request.Query["userId"].ToString();
        if (string.IsNullOrEmpty(userId))
        {
            context.Response.StatusCode = 400;
            return;
        }

        var roomId = context.Request.Query["roomId"].ToString();
        if (string.IsNullOrEmpty(roomId))
        {
            context.Response.StatusCode = 400;
            return;
        }

        if (!int.TryParse(roomId, out var roomIdInt))
        {
            context.Response.StatusCode = 400;
            return;
        }

        if (context.WebSockets.IsWebSocketRequest)
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            WebSocketConnectionManager.AddUser(roomIdInt, userId, webSocket);

            await HandleStreamWebSocketAsync(context, webSocket, roomIdInt, userId);
            WebSocketConnectionManager.RemoveUser(roomIdInt, userId);
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }

    private static async Task HandleStreamWebSocketAsync(HttpContext context, WebSocket webSocket, int roomId, string userId)
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

                    await HandleCommandAsync(message, roomId, userId);

                    messageBuilder.Clear();
                }
            }
            else if (result.MessageType == WebSocketMessageType.Binary)
            {
                await WebSocketConnectionManager.BroadcastBinaryAsync(buffer, result, roomId, userId);
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            }

            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        WebSocketConnectionManager.RemoveUser(roomId, userId);
    }

    private static async Task HandleCommandAsync(string message, int roomId, string userId)
    {
        var parts = message.Split(';');
        var command = parts[0];

        switch (command)
        {
            case "REQUEST_MIC_STATUS":
                await WebSocketConnectionManager.BroadcastMessageAsync("requestedMicStatus", roomId, userId);
                break;
            case "MIC_STATUS":
                var isOn = parts[1] == "on";
                await WebSocketConnectionManager.BroadcastMessageAsync($"microphoneStatus;{userId};{isOn}", roomId, userId);
                break;
            case "JOINED":
                await WebSocketConnectionManager.BroadcastMessageAsync($"joined;{userId};User {userId} has joined the chat", roomId, userId);
                break;
            case "LEAVED":
                await WebSocketConnectionManager.BroadcastMessageAsync($"leaved;{userId};User {userId} leaved from chat", roomId, userId);
                break;
            default:
                // Handle unknown commands if necessary
                break;
        }
    }
}
