using CombatAnalysis.ChatAPI.Consts;
using CombatAnalysis.ChatAPI.Interfaces;
using Livekit.Server.Sdk.Dotnet;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.ChatAPI.Services;

internal class LiveKitService(IOptions<LiveKitOptions> options) : ILiveKitService
{
    private readonly LiveKitOptions _options = options.Value;

    public string GenerateToken(string roomName, string participantIdentity, string participantName)
    {
        var token = new AccessToken(
                _options.ApiKey,
                _options.ApiSecret)
            .WithIdentity(participantIdentity)
            .WithName(participantName)
            .WithGrants(new VideoGrants
            {
                RoomJoin = true,
                Room = roomName,
                CanPublish = true,
                CanSubscribe = true
            })
            .WithTtl(TimeSpan.FromHours(1));

        return token.ToJwt();
    }
}
