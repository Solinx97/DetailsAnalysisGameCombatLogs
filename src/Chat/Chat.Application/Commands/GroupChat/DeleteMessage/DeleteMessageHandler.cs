using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;

namespace Chat.Application.Commands.GroupChat.DeleteMessage;

internal class DeleteMessageHandler(IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteMessageCommand>
{
    private readonly IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetByIdAsync(request.ChatId);
        chat.RemoveMessage(request.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

