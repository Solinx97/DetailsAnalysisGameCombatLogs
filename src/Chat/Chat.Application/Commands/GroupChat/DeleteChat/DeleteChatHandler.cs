using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using MediatR;

namespace Chat.Application.Commands.GroupChat.DeleteChat;

internal class DeleteChatHandler(IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteChatCommand>
{
    private readonly IGenericRepository<Domain.Aggregates.GroupChat, GroupChatId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteChatCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
