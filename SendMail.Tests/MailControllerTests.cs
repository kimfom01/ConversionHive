using Microsoft.AspNetCore.Mvc;
using Moq;
using SendMail.Controllers;
using SendMail.Models.Mail;
using SendMail.Services;

namespace SendMail.Tests;

public class MailControllerTests
{
    private readonly Mock<IMailService> _mailService;
    private readonly MailController _mailController;

    public MailControllerTests()
    {
        _mailService = new Mock<IMailService>();
        _mailController = new MailController(_mailService.Object);
    }

    [Fact]
    public async void SendMail_WhenCalled_ReturnsCreatedAtActionResult()
    {
        _mailService.Setup(s => s.SendMail(It.IsAny<MailDto>())).ReturnsAsync(new Mock<Mail>().Object);

        var createdResponse = await _mailController.SendMail(new Mock<MailDto>().Object);

        Assert.IsType<CreatedAtActionResult>(createdResponse);
    }

    [Fact]
    public async void SendMail_WhenCalled_ReturnsBadRequestObjectResult()
    {
        _mailService.Setup(s => s.SendMail(It.IsAny<MailDto>())).ReturnsAsync(() => null);

        var badRequestResponse = await _mailController.SendMail(new Mock<MailDto>().Object);

        Assert.IsType<BadRequestResult>(badRequestResponse);
    }

    [Fact]
    public async void GetSavedMail_WhenCalled_ReturnsOkObjectResult()
    {
        _mailService.Setup(s => s.GetSavedMail(It.IsAny<int>())).ReturnsAsync(new Mock<MailDto>().Object);

        var okResult = await _mailController.GetSavedMail(It.IsAny<int>());

        Assert.IsType<OkObjectResult>(okResult);
    }

    [Fact]
    public async void GetSavedMail_WhenCalled_ReturnsNotFoundResult()
    {
        _mailService.Setup(s => s.GetSavedMail(It.IsAny<int>())).ReturnsAsync(() => null);

        var notFound = await _mailController.GetSavedMail(It.IsAny<int>());

        Assert.IsType<NotFoundResult>(notFound);
    }
}