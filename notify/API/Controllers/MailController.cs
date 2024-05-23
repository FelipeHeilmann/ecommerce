﻿using API.Events;
using API.Gateway;
using API.Request;
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
        public async Task<ActionResult> SendWelcomeMail([FromBody] WelcomeRequest request)
        {
            var mailData = new Maildata()
            {
                EmailBody = Templates.Welcome(request.Name),
                EmailSubject = "Welcome",
                EmailToEmail = request.Email,
                EmailToName = request.Name,
            };

            await _mailerGateway.Send(mailData);

            return Ok();
        }

        [HttpPost("order-created")]
        public async Task<ActionResult> SendOrderCreatedMail([FromBody] OrderCheckedoutEvent request)
        {
            var mailData = new Maildata()
            {
                EmailToEmail = request.Email,
                EmailToName = request.Name,
                EmailSubject = "Order Created",
                EmailBody = Templates.OrderCreated(request)
            };

            await _mailerGateway.Send(mailData);

            return Ok();
        }
    }
}
