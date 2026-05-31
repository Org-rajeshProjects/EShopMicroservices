namespace BuildingBlocks.Messaging.Events;

//Integration events are used to communicate between different bounded contexts or microservices. They represent significant occurrences in the system that other parts of the system might be interested in. By using integration events, we can achieve loose coupling between different parts of the system and enable asynchronous communication.
public record IntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();// Unique identifier for the event
    public DateTime OccurredOn { get; init; } = DateTime.Now;// Timestamp of when the event occurred

    public string EventType => GetType().AssemblyQualifiedName;// The type of the event, used for deserialization
}