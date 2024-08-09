using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace CombatAnalysis.ChatApi.Core;

public class WebSocketConnectionManager
{
    private static ConcurrentDictionary<string, WebSocket> _connectedUsers = new ConcurrentDictionary<string, WebSocket>();

    public static void AddUser(string userId, WebSocket webSocket)
    {
        _connectedUsers.TryAdd(userId, webSocket);
    }

    public static IDictionary<string, WebSocket> GetConnectedSockets()
    {
        return _connectedUsers;
    }

    public static IEnumerable<string> GetOnlyConnectedUsers()
    {
        return _connectedUsers.Keys.AsEnumerable();
    }

    public static void RemoveUser(string userId)
    {
        _connectedUsers.TryRemove(userId, out _);
    }

    public static async Task BroadcastMessageAsync(string message, string userId, bool sendToMeToo = false)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        var segment = new ArraySegment<byte>(buffer);

        foreach (var socket in _connectedUsers)
        {
            if (socket.Value.State == WebSocketState.Open)
            {
                if ((!sendToMeToo && socket.Key != userId) || sendToMeToo)
                {
                    await socket.Value.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }

    public static async Task BroadcastBinaryAsync(byte[] data, WebSocketReceiveResult result, string userId, bool sendToMeToo = false)
    {
        var messageSegment = new ArraySegment<byte>(data, 0, result.Count);

        foreach (var socket in _connectedUsers)
        {
            if (socket.Value.State == WebSocketState.Open)
            {
                if ((!sendToMeToo && socket.Key != userId) || sendToMeToo)
                {
                    await socket.Value.SendAsync(messageSegment, result.MessageType, result.EndOfMessage, CancellationToken.None);
                }
            }
        }
    }
}
