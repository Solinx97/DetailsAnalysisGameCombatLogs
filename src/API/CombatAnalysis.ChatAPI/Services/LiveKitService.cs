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

    public async Task<int> GetParticipantsCountAsync(string voiceChatId)
    {
        var roomService = new RoomServiceClient(
            _options.Url,
            _options.ApiKey,
            _options.ApiSecret);

        var roomsResponse = await roomService.ListRooms(new ListRoomsRequest
        {
            Names = { $"voice-chat-{voiceChatId}" }
        });

        var roomExists = roomsResponse.Rooms
            .Any(room => room.Name == $"voice-chat-{voiceChatId}");

        if (!roomExists)
        {
            return 0;
        }

        var response = await roomService.ListParticipants(new ListParticipantsRequest
        {
            Room = $"voice-chat-{voiceChatId}"
        });

        var participantCount = response.Participants.Count;

        return participantCount;
    }
}
