using Chat.Domain.Aggregates;

namespace Chat.Domain.Repositories;

public interface IVoiceChatRepository
{
    Task AddAsync(VoiceChat chat, CancellationToken cancelationToken);

    Task<VoiceChat?> GetByChatIdAsync(int chatId, CancellationToken cancelationToken);

    Task<bool> IsChatExistAsync(int chatId, CancellationToken cancelationToken);
}
