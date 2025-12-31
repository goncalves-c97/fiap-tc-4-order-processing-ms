using Core.Dtos;
using Core.Interfaces;
using Core.UseCases;

namespace Core.Controllers
{
    public static class EmailController
    {
        public static async Task SendEmailAsync(IEmailService emailService, EmailRequestDto emailRequestDto)
        {
            await emailService.SendEmailAsync(emailRequestDto);
        }

        public static async Task SendPingEmail(IEmailService emailService, string email)
        {
            await EmailUseCases.SendEmailPing(emailService, email);
        }
    }
}
