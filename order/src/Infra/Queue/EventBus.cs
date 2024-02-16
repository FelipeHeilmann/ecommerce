using Application.Abstractions.Queue;
using MassTransit;

namespace Infra.Queue;

public class EventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublicAsync<T>(T messagem, CancellationToken cancellationToken) where T : class
    {
         return _publishEndpoint.Publish<T>(messagem, cancellationToken);
    }
}
