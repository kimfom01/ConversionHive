using Moq;
using SendMail.Controllers;
using SendMail.Repository;
using SendMail.Services;

namespace SendMail.Tests;

public class MailControllerTests
{
    private readonly MailController _mailController;

    public MailControllerTests()
    {
        var mockMailer = new Mock<IMailer>();
        var mockRepository = new Mock<IMailRepository>();
        _mailController = new MailController(mockMailer.Object, mockRepository.Object);
    }

    [Fact]
    public Task SendMail_WhenCalled_ReturnsOkResult()
    {
        return Task.CompletedTask;
    }
}
