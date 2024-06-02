namespace NotifyWorker.Gateway;

public interface IMailerGateway
{
    public Task Send(Maildata request);
}
