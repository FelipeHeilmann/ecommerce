namespace NotifyWorker.Events;

public sealed record CustomerCreatedEvent(string Name, string Email);
