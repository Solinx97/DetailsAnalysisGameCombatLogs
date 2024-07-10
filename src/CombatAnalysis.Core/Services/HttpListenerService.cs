using System.Net;

namespace CombatAnalysis.Core.Services;

internal class HttpListenerService
{
    private readonly HttpListener _listener = new HttpListener();

    public HttpListenerService(string listeningUrl)
    {
        _listener.Prefixes.Add(listeningUrl);
    }

    public async Task StartListeningAsync(Action<string, string> onCallbackReceived)
    {
        try
        {
            _listener.Start();
            while (_listener.IsListening)
            {
                var context = await _listener.GetContextAsync();
                var authorizationCode = context.Request.QueryString["code"];
                var incomingState = context.Request.QueryString["state"];

                onCallbackReceived?.Invoke(authorizationCode, incomingState);

                var response = context.Response;
                string responseString = "<html><body>You can close this window.</body></html>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;

                var output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();

                StopListening();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting HttpListener: {ex.Message}");
            StopListening();
        }
    }

    private void StopListening()
    {
        _listener.Stop();
    }
}
