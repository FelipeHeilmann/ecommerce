namespace API.gateway;

public interface IMailerGateway
{
    public Task Send(string email, string name);
}
