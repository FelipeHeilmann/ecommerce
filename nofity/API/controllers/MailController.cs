using API.gateway;
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

        [HttpPost]
        public async Task<ActionResult> SendMail([FromBody] EmailRequest request)
        {
            await _mailerGateway.Send(request.name, request.email);

            return Ok();
        }
    }


    public record EmailRequest(string email, string name) { }
}
