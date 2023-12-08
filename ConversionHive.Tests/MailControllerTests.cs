using Microsoft.AspNetCore.Mvc;
using Moq;
using ConversionHive.Controllers;
using ConversionHive.Services;

namespace ConversionHive.Tests;

public class MailControllerTests
{
    private readonly Mock<IMailService> _mailService;
    private readonly MailController _mailController;

    public MailControllerTests()
    {
        _mailService = new Mock<IMailService>();
        _mailController = new MailController(_mailService.Object);
    }

    // [Fact]
    // public async void SendMail_WhenCalled_ReturnsCreatedAtActionResult()
    // {
    //     _mailService.Setup(s => s.SendMail(It.IsAny<MailDto>())).ReturnsAsync(new Mock<Mail>().Object);
    //
    //     var createdResponse = await _mailController.SendMail(new Mock<MailDto>().Object);
    //
    //     Assert.IsType<CreatedAtActionResult>(createdResponse);
    // }
    //
    // [Fact]
    // public async void SendMail_WhenCalled_ReturnsBadRequestObjectResult()
    // {
    //     _mailService.Setup(s => s.SendMail(It.IsAny<MailDto>())).ReturnsAsync(() => null);
    //
    //     var badRequestResponse = await _mailController.SendMail(new Mock<MailDto>().Object);
    //
    //     Assert.IsType<BadRequestResult>(badRequestResponse);
    // }
}