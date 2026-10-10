using Chat.Domain.Aggregates;
using Chat.Domain.Repositories;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class VoiceChatRepository(ChatContext context) : IVoiceChatRepository
{
    private readonly ChatContext _context = context;

    public async Task AddAsync(VoiceChat chat, CancellationToken cancelationToken)
    {
        await _context.VoiceChat
                     .AddAsync(chat, cancelationToken);
    }

    public async Task<VoiceChat?> GetByChatIdAsync(int chatId, CancellationToken cancelationToken)
    {
        var chat = await _context.VoiceChat
                    .FirstOrDefaultAsync(x => x.GroupChatId == chatId, cancelationToken);

        return chat;
    }

    public async Task<bool> IsChatExistAsync(int chatId, CancellationToken cancelationToken)
    {
        var count = await _context.VoiceChat
                    .CountAsync(x => x.GroupChatId == chatId, cancelationToken);

        return count > 0;
    }
}
