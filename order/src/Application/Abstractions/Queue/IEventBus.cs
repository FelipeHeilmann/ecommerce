namespace Application.Abstractions.Queue;

public interface IEventBus
{
    Task PublicAsync<T>(T messagem, CancellationToken cancellationToken) where T : class; 
}
