using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;

namespace Chat.Application.Commands.GroupChat.IncreaseUnreadMessagesCount;

internal class IncreaseUnreadMessagesCountHandler(IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> repository, IUnitOfWork unitOfWork) : IRequestHandler<IncreaseUnreadMessagesCountCommand>
{
    private readonly IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(IncreaseUnreadMessagesCountCommand request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetByIdAsync(request.ChatId);
        //chat.UpdateName(request.Name);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

