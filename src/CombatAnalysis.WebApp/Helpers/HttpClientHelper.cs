using CombatAnalysis.WebApp.Interfaces;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace CombatAnalysis.WebApp.Helpers;

internal class HttpClientHelper : IHttpClientHelper
{
    private const string _baseAddressApi = "api/v1/";

    public HttpClientHelper()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                if (policyErrors == SslPolicyErrors.None)
                {
                    return true; // If there's no error, proceed.
                }

                var caCert = new X509Certificate2("/etc/ssl/certs/ca-cert/ca.crt");
                var chain = new X509Chain();
                chain.ChainPolicy.ExtraStore.Add(caCert);
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;
                chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;

                return chain.Build(cert ?? new X509Certificate2(new byte[] { 1 })); // Validate the certificate against the CA.
            }
        };

        Client = new HttpClient(handler);

    }

    public HttpClient Client { get; set; }

    public string BaseAddress { get; set; } = string.Empty;

    public async Task<HttpResponseMessage> PostAsync(string requestUri, JsonContent content)
    {
        var result = await Client.PostAsync($"{BaseAddress}{_baseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        var result = await Client.GetAsync($"{BaseAddress}{_baseAddressApi}{requestUri}");

        return result;
    }

    public async Task<HttpResponseMessage> PutAsync(string requestUri, JsonContent content)
    {
        var result = await Client.PutAsync($"{BaseAddress}{_baseAddressApi}{requestUri}", content);

        return result;
    }

    public async Task<HttpResponseMessage> DeletAsync(string requestUri)
    {
        var result = await Client.DeleteAsync($"{BaseAddress}{_baseAddressApi}{requestUri}");

        return result;
    }
}
