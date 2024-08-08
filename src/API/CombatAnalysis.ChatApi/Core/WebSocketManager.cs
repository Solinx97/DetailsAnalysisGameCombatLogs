using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace CombatAnalysis.ChatApi.Core;

public class WebSocketManager
{
    private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();

    public void AddSocket(string connectionId, WebSocket socket)
    {
        _sockets[connectionId] = socket;
    }

    public async Task RemoveSocket(string connectionId)
    {
        if (_sockets.TryRemove(connectionId, out var socket))
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the WebSocketManager", CancellationToken.None);
        }
    }

    public async Task SendMessageAsync(string connectionId, string message)
    {
        if (_sockets.TryGetValue(connectionId, out var socket) && socket.State == WebSocketState.Open)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
