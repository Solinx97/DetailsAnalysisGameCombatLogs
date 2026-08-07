using Communication.Domain.Enums;
using Communication.Domain.Exceptions;

namespace Communication.Infrastruction.Exceptions;

public class EntityNotFoundException(Type entityType, object entityId) : DomainException($"Entity '{entityType.Name}' with Id '{entityId}' was not found.", ExceptionCode.NotFound)
{
    public Type EntityType { get; } = entityType;

    public object EntityId { get; } = entityId;
}
