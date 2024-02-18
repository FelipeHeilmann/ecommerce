namespace Application.Abstractions.Queue
{
    public interface IQueue
    {
        void On();
        void Publish(object message, string queue);

        void Consume(string queue);
    }
}
