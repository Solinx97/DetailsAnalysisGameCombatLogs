using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace CombatAnalysis.ChatApi.Core;

public class WebSocketConnectionManager
{
    private static readonly ConcurrentDictionary<int, ConcurrentDictionary<string, WebSocket>> _rooms = new();

    public static void AddUser(int roomId, string userId, WebSocket webSocket)
    {
        if (!_rooms.ContainsKey(roomId))
        {
            _rooms[roomId] = new ConcurrentDictionary<string, WebSocket>();
        }

        _rooms[roomId][userId] = webSocket;
    }

    public static IEnumerable<string> GetConnectedUsers(int roomId)
    {
        if (_rooms.TryGetValue(roomId, out var users))
        {
            return users.Keys.AsEnumerable();
        }

        return new List<string>();
    }

    public static void RemoveUser(int roomId, string userId)
    {
        if (_rooms.ContainsKey(roomId))
        {
            _rooms[roomId].TryRemove(userId, out _);

            if (_rooms[roomId].IsEmpty)
            {
                _rooms.TryRemove(roomId, out _);
            }
        }
    }

    public static async Task BroadcastMessageAsync(string message, int roomId, string userId, bool sendToMeToo = false)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        var segment = new ArraySegment<byte>(buffer);

        foreach (var socket in _rooms[roomId])
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

    public static async Task BroadcastBinaryAsync(byte[] buffer, WebSocketReceiveResult result, int roomId, string userId, bool sendToMeToo = false)
    {
        var messageSegment = new ArraySegment<byte>(buffer, 0, result.Count);

        foreach (var socket in _rooms[roomId])
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
