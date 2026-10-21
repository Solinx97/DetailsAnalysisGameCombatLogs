namespace CombatAnalysis.ChatAPI.Interfaces;

public interface ILiveKitService
{
    string GenerateToken(string roomName, string participantIdentity, string participantName);
}
