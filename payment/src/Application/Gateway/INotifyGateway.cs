namespace Application.Gateway;

public interface INotifyGateway
{
    public Task SendPaymentRecivedMail(PaymenRecivedRequest payment);
}

public record PaymenRecivedRequest(string Name, string Email, Guid OrderId, double Amount);
