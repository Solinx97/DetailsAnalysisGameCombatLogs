using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;

namespace Chat.Application.Commands.GroupChat.UpdateMessageText;

internal class UpdateMessageTextHandler(IGenericRepository<GroupChatMessage, GroupChatMessageId> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateMessageTextCommand>
{
    private readonly IGenericRepository<GroupChatMessage, GroupChatMessageId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateMessageTextCommand request, CancellationToken cancellationToken)
    {
        var message = await _repository.GetByIdAsync(request.Id);
        message.EditMessage(request.Message);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
