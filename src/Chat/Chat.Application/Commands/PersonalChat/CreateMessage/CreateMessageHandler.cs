using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Commands.PersonalChat.CreateMessage;

internal class CreateMessageHandler(IPersonalChatMessageRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateMessageCommand>
{
    private readonly IPersonalChatMessageRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var message = PersonalChatMessage.Create(request.Username, request.Message, request.ChatId, request.AppUserId);
        await _repository.AddAsync(message, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
