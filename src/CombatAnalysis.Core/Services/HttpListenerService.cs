using Microsoft.Extensions.Logging;
using System.Net;

namespace CombatAnalysis.Core.Services;

internal class HttpListenerService
{
    private readonly HttpListener _listener = new();
    private readonly ILogger _logger;

    public HttpListenerService(string listeningUrl, ILogger logger)
    {
        _logger = logger;
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
                var request = context.Request;
                var response = context.Response;

                var authorizationCode = request.QueryString["code"];
                var state = request.QueryString["state"];
                if (authorizationCode == null || state == null)
                {
                    StopListening();

                    return;
                }

                onCallbackReceived(authorizationCode, state);

                string responseString = "<html><body>You can close this window.</body></html>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;

                var responseOutput = response.OutputStream;
                responseOutput.Write(buffer, 0, buffer.Length);
                responseOutput.Close();

                StopListening();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            StopListening();
        }
    }

    private void StopListening()
    {
        _listener.Stop();
    }
}
