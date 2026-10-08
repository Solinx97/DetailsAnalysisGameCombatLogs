using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;

namespace Chat.Application.Commands.PersonalChat.UpdateMessageText;

internal class UpdateMessageTextHandler(IGenericRepository<PersonalChatMessage, PersonalChatMessageId> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateMessageTextCommand>
{
    private readonly IGenericRepository<PersonalChatMessage, PersonalChatMessageId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateMessageTextCommand request, CancellationToken cancellationToken)
    {
        var message = await _repository.GetByIdAsync(request.Id);
        message.EditMessage(request.Message);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
