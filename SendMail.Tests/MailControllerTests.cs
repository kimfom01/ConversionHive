using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SendMail.Controllers;
using SendMail.Models;
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
    public async Task SendMail_WhenCalled_ReturnsCreatedAtActionResult()
    {
        var mail = new SendMailDto
        {
            Name = "Sender",
            Email = "sender@sendmail.com",
            RecipientEmail = "recipient@recipient.com",
            Subject = "This is my subject",
            Body = "This is the message body"
        };
        
        var createdResponse = await _mailController.SendMail(mail);

        Assert.IsType<CreatedAtActionResult>(createdResponse);
    }

    [Fact]
    public async Task SendMail_WhenCalled_ReturnsBadRequestObjectResult()
    {
        var mail = new SendMailDto
        {
            RecipientEmail = "recipient@recipient.com",
            Subject = "This is my subject",
            Body = "This is the message body"
        };

        var badRequestResponse = await _mailController.SendMail(mail);

        Assert.IsType<BadRequestObjectResult>(badRequestResponse);
    }

    [Fact]
    public async Task GetSavedMail_WhenCalled_ReturnsOkObjectResult()
    {
        var okResult = await _mailController.GetSavedMail(1);

        Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
    }
    
    [Fact]
    public async Task GetSavedMail_WhenCalled_ReturnsNotFoundResult()
    {
        var notFound = await _mailController.GetSavedMail(3);

        Assert.IsType<NotFoundResult>(notFound);
    }
}
