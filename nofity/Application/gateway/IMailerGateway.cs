namespace Application.gateway;

public interface IMailerGateway
{
    public Task Send(string email);
}
