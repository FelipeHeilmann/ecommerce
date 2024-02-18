using Application.Abstractions.Queue;

namespace Infra.Queue;

public class MemoryMQAdapter : IQueue
{
    public void On()
    {
        return;
    }

    public void Consume(string queue)
    {
        return;
    }

    public void Publish(object message, string queue)
    {
        return;
    }
}
