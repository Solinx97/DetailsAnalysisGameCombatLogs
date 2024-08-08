using CombatAnalysis.ChatApi.Core;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace CombatAnalysis.ChatApi.Middlewares;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;
    private static ConcurrentDictionary<string, WebSocket> _connectedUsers = new ConcurrentDictionary<string, WebSocket>();

    public WebSocketMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/ws")
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                WebSocketConnectionManager.AddSocket(webSocket);

                await HandleWebSocketAsync(context, webSocket);
                WebSocketConnectionManager.RemoveSocket(webSocket);
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

    public static async Task BroadcastMessageAsync(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        var segment = new ArraySegment<byte>(buffer);

        foreach (var socket in WebSocketConnectionManager.GetAllSockets())
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    private static async Task HandleWebSocketAsync(HttpContext context, WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (!result.CloseStatus.HasValue)
        {
            // Echo the message back to the client
            await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }

        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
}
