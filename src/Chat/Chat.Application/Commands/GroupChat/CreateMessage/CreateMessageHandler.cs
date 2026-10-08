using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Commands.GroupChat.CreateMessage;

internal class CreateMessageHandler(IGroupChatMessageRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateMessageCommand>
{
    private readonly IGroupChatMessageRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var message = GroupChatMessage.Create(request.Username, request.Message, request.ChatId, request.GroupChatUserId);
        await _repository.AddAsync(message, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
