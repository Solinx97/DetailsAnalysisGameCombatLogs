using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace CombatAnalysis.ChatApi.Core;

public class WebSocketConnectionManager
{
    //private static ConcurrentDictionary<WebSocket> _sockets = new ConcurrentDictionary<WebSocket>();
    private static ConcurrentDictionary<string, WebSocket> _connectedUsers = new ConcurrentDictionary<string, WebSocket>();

    public static void AddUser(string userId, WebSocket webSocket)
    {
        //_sockets.Add(webSocket);
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

    public static void RemoveUser(string userId, WebSocket webSocket)
    {
        //_sockets.Remove(webSocket);
        _connectedUsers.TryRemove(userId, out _);
    }

    public static async Task NotifyMicrophoneStatusChangeAsync(string userId, bool isMicrophoneOn)
    {
        string message = $"microphoneStatus;{userId};{isMicrophoneOn};User {userId} has turned their microphone {(isMicrophoneOn ? "on" : "off")}.";
        await BroadcastMessageAsync(message);
    }

    public static async Task BroadcastMessageAsync(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        var segment = new ArraySegment<byte>(buffer);

        foreach (var socket in _connectedUsers)
        {
            if (socket.Value.State == WebSocketState.Open)
            {
                await socket.Value.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
