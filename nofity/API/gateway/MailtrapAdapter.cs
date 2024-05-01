using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace API.gateway
{
    public class MailtrapAdapter : IMailerGateway
    {
        private readonly MailtrapSettings _mailSettings;
        public MailtrapAdapter(IOptions<MailtrapSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }

        public async Task Send(string email, string name)
        {      
          
            using (MimeMessage emailMessage = new MimeMessage())
            {
                MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderMail);
                emailMessage.From.Add(emailFrom);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(name, email);
                emailMessage.To.Add(emailTo);

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = "Corpo do email";

                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                
                using (SmtpClient mailClient = new SmtpClient())
                {
                    await mailClient.ConnectAsync(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    await mailClient.AuthenticateAsync(_mailSettings.Username, _mailSettings.Password);
                    await mailClient.SendAsync(emailMessage);
                    await mailClient.DisconnectAsync(true);
                }
            }

        }
    }
}
