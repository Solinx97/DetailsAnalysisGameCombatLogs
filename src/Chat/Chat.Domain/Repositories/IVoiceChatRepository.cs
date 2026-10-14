using Chat.Domain.Aggregates;
using Chat.Domain.ValueObjects;

namespace Chat.Domain.Repositories;

public interface IVoiceChatRepository
{
    Task AddAsync(VoiceChat chat, CancellationToken cancelationToken);

    Task<VoiceChat?> GetByChatIdAsync(GroupChatId chatId, CancellationToken cancelationToken);

    Task<bool> IsChatExistAsync(GroupChatId chatId, CancellationToken cancelationToken);
}
