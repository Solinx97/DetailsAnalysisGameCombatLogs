using System.Net.WebSockets;

namespace CombatAnalysis.ChatApi.Core;

public class WebSocketConnectionManager
{
    private static readonly List<WebSocket> _sockets = new List<WebSocket>();

    public static void AddSocket(WebSocket socket)
    {
        _sockets.Add(socket);
    }

    public static void RemoveSocket(WebSocket socket)
    {
        _sockets.Remove(socket);
    }

    public static IEnumerable<WebSocket> GetAllSockets()
    {
        return _sockets;
    }
}
