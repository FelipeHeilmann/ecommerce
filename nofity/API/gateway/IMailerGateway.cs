namespace API.gateway;

public interface IMailerGateway
{
    public Task Send(Maildata request);
}
