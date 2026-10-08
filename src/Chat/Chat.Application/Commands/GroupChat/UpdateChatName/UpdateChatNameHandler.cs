using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;

namespace Chat.Application.Commands.GroupChat.UpdateChatName;

internal class UpdateChatNameHandler(IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateChatNameCommand>
{
    private readonly IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateChatNameCommand request, CancellationToken cancellationToken)
    {
        var chat = await _repository.GetByIdAsync(request.Id);
        chat.UpdateName(request.Name);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
