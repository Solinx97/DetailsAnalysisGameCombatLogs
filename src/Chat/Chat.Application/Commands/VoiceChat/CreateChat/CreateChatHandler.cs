using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Commands.VoiceChat.CreateChat;

internal class CreateChatHandler(IVoiceChatRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateChatCommand>
{
    private readonly IVoiceChatRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var chat = Domain.Aggregates.VoiceChat.Create(request.GroupChatId, request.AppUserId);
        await _repository.AddAsync(chat, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
