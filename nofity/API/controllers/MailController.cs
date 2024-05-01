using API.Gateway;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.controllers
{
    [Route("api/notify")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailerGateway _mailerGateway;
        public MailController(IMailerGateway mailerGateway)
        {
            _mailerGateway = mailerGateway;
        }

        [HttpPost("welcome")]
        public async Task<ActionResult> SendMail([FromBody] EmailRequest request)
        {
            var mailData = new Maildata()
            {
                EmailBody = Templates.Welcome(request.name),
                EmailSubject = "Welcome",
                EmailToEmail = request.email,
                EmailToName = request.name,
            };

            await _mailerGateway.Send(mailData);

            return Ok();
        }
    }


    public record EmailRequest(string email, string name) { }
}
