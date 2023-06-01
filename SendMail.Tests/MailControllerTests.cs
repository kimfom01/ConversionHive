using FluentEmail.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SendMail.Controllers;
using SendMail.Models;
using SendMail.Services;

namespace SendMail.Tests;

public class MailControllerTests
{
    private readonly Mock<IMailer> _mockMailer;
    private readonly MailController _mailController;

    public MailControllerTests()
    {
        _mockMailer = new Mock<IMailer>();
        _mailController = new MailController(_mockMailer.Object);
    }

    [Fact]
    public Task SendMail_WhenCalled_ReturnsOkResult()
    {
        return Task.CompletedTask;
    }
}
