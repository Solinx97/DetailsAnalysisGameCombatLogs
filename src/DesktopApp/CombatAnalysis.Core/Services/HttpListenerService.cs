using Microsoft.Extensions.Logging;
using System.Net;

namespace CombatAnalysis.Core.Services;

internal class HttpListenerService(ILogger logger)
{
    private HttpListener? _listener;
    private readonly ILogger _logger = logger;

    public async Task StartListeningAsync(string listeningUrl, Action<string, string> onCallbackReceived, CancellationToken cancellationToken)
    {
        try
        {
            using var registration = cancellationToken.Register(() =>
            {
                if (_listener?.IsListening == true)
                {
                    _listener.Stop();
                }
            });

            _listener = new HttpListener();
            _listener.Prefixes.Add(listeningUrl);
            _listener.Start();

            var context = await _listener.GetContextAsync();

            HandleRequest(onCallbackReceived, context);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Listening cancelled");
        }
        catch (HttpListenerException ex) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Listener stopped");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        finally
        {
            _listener?.Close();
            _listener = null;
        }
    }

    private static void HandleRequest(Action<string, string> onCallbackReceived, HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        var authorizationCode = request.QueryString["code"];
        var state = request.QueryString["state"];
        if (authorizationCode == null || state == null)
        {
            return;
        }

        onCallbackReceived(authorizationCode, state);

        var responseString = GetHtmlContent();

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;

        var cookie = new Cookie("idsrv", "")
        {
            Expires = DateTime.UtcNow.AddDays(-1),
            Path = "/"
        };

        response.Cookies.Add(cookie);

        var responseOutput = response.OutputStream;
        responseOutput.Write(buffer, 0, buffer.Length);
        responseOutput.Close();
    }

    private static string GetHtmlContent()
    {
        string responseString = """
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset="utf-8">
                    <title>Authorization</title>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            background-color: #0c192c;
                            display: flex;
                            justify-content: center;
                            align-items: center;
                            height: 100vh;
                            margin: 0;
                        }

                        .container {
                            background: white;
                            padding: 30px;
                            border-radius: 12px;
                            box-shadow: 0 4px 20px rgba(0,0,0,0.15);
                            text-align: center;
                        }

                        .success {
                            color: #2e7d32;
                            font-size: 24px;
                        }

                        .message {
                            color: #555;
                            margin-top: 10px;
                        }
                    </style>
                </head>

                <body>
                    <div class="container">
                        <div class="success">
                            Authorization completed
                        </div>

                        <div class="message">
                            You can close this window.
                        </div>
                    </div>
                </body>
                </html>
                """;

        return responseString;
    }
}
