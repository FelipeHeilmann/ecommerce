namespace Domain.Abstractions;

public interface IDomainEvent
{
    string EventName { get; }
    object Data { get; }
}