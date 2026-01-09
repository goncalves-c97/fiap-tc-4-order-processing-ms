using System.Security.Claims;
using Core.Dtos;
using Core.Interfaces;
using Core.UseCases;
using FluentAssertions;
using Moq;

namespace Test.Core.UseCases;

public class EmailUseCasesTests
{
    [Fact]
    public async Task SendEmail_Should_ValidateArgs()
    {
        await FluentActions.Invoking(() => EmailUseCases.SendEmail(null!, new EmailRequestDto())).Should().ThrowAsync<ArgumentNullException>();
        await FluentActions.Invoking(() => EmailUseCases.SendEmail(new Mock<IEmailService>().Object, null!)).Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task SendEmail_Should_CallService()
    {
        var svc = new Mock<IEmailService>(MockBehavior.Strict);
        svc.Setup(s => s.SendEmailAsync(It.IsAny<EmailRequestDto>())).Returns(Task.CompletedTask);

        await EmailUseCases.SendEmail(svc.Object, new EmailRequestDto { ToEmail = "a@b.com", Subject = "s", Body = "b" });
        svc.VerifyAll();
    }

    [Fact]
    public void GetClaimValue_Should_ReturnNull_WhenUnreadableToken()
    {
        EmailUseCases.GetClaimValue("not-a-jwt", ClaimTypes.Email).Should().BeNull();
    }
}
