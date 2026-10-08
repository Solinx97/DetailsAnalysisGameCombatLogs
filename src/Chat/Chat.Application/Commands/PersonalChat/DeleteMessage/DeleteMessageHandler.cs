using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;

namespace Chat.Application.Commands.PersonalChat.DeleteMessage;

internal class DeleteMessageHandler(IGenericRepository<Domain.Aggregates.PersonalChat, PersonalChatId> repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteMessageCommand>
{
    private readonly IGenericRepository<Domain.Aggregates.PersonalChat, PersonalChatId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetByIdAsync(request.ChatId);
        chat.RemoveMessage(request.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
