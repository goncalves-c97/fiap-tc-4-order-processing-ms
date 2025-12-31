using Core.Controllers;
using Core.Dtos;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints
{
    [ApiController]
    [Route("Email")]
    public class EmailEndpoint : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailEndpoint(IEmailService emailService) => _emailService = emailService;

        [HttpPost, Route("SendPing")]
        public async Task<IActionResult> SendPing([FromQuery] string email)
        {
            await EmailController.SendPingEmail(_emailService, email);
            return Ok();
        }
    }
}
