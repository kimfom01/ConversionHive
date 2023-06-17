using AutoMapper;
using FluentEmail.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SendMail.Controllers;
using SendMail.Models;
using SendMail.Repository;
using SendMail.Services;

namespace SendMail.Tests;

public class MailControllerTests
{
    private readonly Mock<IMailer> _mailer;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IMapper> _mapper;
    private readonly MailController _mailController;

    public MailControllerTests()
    {
        _mailer = new Mock<IMailer>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _mapper = new Mock<IMapper>();
        _mailController = new MailController(_mailer.Object, _unitOfWork.Object, _mapper.Object);
    }

    [Fact]
    public async void SendMail_WhenCalled_ReturnsCreatedAtActionResult()
    {
        _mailer.Setup(s => s.SendMail(It.IsAny<Mail>())).ReturnsAsync(true);
        var mail = _mapper.Setup(m => m.Map<Mail>(It.IsAny<SendMailDto>())).Returns(new Mock<Mail>().Object);
        _unitOfWork.Setup(u => u.Mails.AddItem(It.IsAny<Mail>())).ReturnsAsync(new Mock<Mail>().Object);
        
        var createdResponse = await _mailController.SendMail(new Mock<SendMailDto>().Object);

        Assert.IsType<CreatedAtActionResult>(createdResponse);
    }

    [Fact]
    public async void SendMail_WhenCalled_ReturnsBadRequestObjectResult()
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
    public async void GetSavedMail_WhenCalled_ReturnsOkObjectResult()
    {
        var okResult = await _mailController.GetSavedMail(1);

        Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
    }
    
    [Fact]
    public async void GetSavedMail_WhenCalled_ReturnsNotFoundResult()
    {
        var notFound = await _mailController.GetSavedMail(3);

        Assert.IsType<NotFoundResult>(notFound);
    }
}
