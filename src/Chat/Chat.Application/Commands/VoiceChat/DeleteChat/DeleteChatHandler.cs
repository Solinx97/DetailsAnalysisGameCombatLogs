using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;

namespace Chat.Application.Commands.VoiceChat.DeleteChat;

internal class DeleteChatHandler(IGenericRepository<Domain.Aggregates.VoiceChat, VoiceChatId> repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteChatCommand>
{
    private readonly IGenericRepository<Domain.Aggregates.VoiceChat, VoiceChatId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteChatCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
