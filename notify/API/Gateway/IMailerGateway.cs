namespace API.Gateway;

public interface IMailerGateway
{
    public Task Send(Maildata request);
}
