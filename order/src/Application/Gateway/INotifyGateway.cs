namespace Application.Gateway;

public interface INotifyGateway
{
    public Task SendMail(string name, string email);
}
