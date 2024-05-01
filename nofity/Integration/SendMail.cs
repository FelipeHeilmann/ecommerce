using Xunit;

namespace Integration;

public class SendMail
{
    [Fact]
    public void Should_Send_Welcome_Email_To_User()
    {
        var email = "felipeheilmannm@gmail.com";
        var mailerGateway = new MailtrapAdapter();
        mailerGateway.Send(email);
    }
}
