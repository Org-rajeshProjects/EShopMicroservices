using MediatR;

namespace Ordering.Domain.Abstractions;

public interface IDomainEvent : INotification//INotification is marker interface for MediatR to identify this as an event that can be published and handled by handlers
{
    Guid EventId => Guid.NewGuid();
    public DateTime OccurredOn => DateTime.Now;
    public string EventType => GetType().AssemblyQualifiedName;
}
