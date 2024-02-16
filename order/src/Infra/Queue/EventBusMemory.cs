using Application.Abstractions.Queue;

namespace Infra.Queue
{
    public class EventBusFake : IEventBus
    {
        public Task PublicAsync<T>(T messagem, CancellationToken cancellationToken) where T : class
        {
            return Task.CompletedTask;
        }
    }
}
