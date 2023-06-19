using AutoMapper;
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
        var mail = _mapper.Setup(m => m.Map<Mail>(It.IsAny<MailDto>())).Returns(new Mock<Mail>().Object);
        _unitOfWork.Setup(u => u.Mails.AddItem(It.IsAny<Mail>())).ReturnsAsync(new Mock<Mail>().Object);

        var createdResponse = await _mailController.SendMail(new Mock<MailDto>().Object);

        Assert.IsType<CreatedAtActionResult>(createdResponse);
    }

    [Fact]
    public async void SendMail_WhenCalled_ReturnsBadRequestObjectResult()
    {
        _mailer.Setup(s => s.SendMail(It.IsAny<Mail>())).ReturnsAsync(false);
        _mapper.Setup(m => m.Map<Mail>(It.IsAny<MailDto>())).Returns(new Mock<Mail>().Object);
        _unitOfWork.Setup(u => u.Mails.AddItem(It.IsAny<Mail>())).ReturnsAsync(new Mock<Mail>().Object);

        var badRequestResponse = await _mailController.SendMail(new Mock<MailDto>().Object);

        Assert.IsType<BadRequestResult>(badRequestResponse);
    }

    [Fact]
    public async void GetSavedMail_WhenCalled_ReturnsOkObjectResult()
    {
        _unitOfWork.Setup(u => u.Mails.GetItem(It.IsAny<int>())).ReturnsAsync(new Mock<Mail>().Object);
        _mapper.Setup(m => m.Map<MailDto>(It.IsAny<Mail>())).Returns(new Mock<MailDto>().Object);

        var okResult = await _mailController.GetSavedMail(It.IsAny<int>());

        Assert.IsType<OkObjectResult>(okResult);
    }

    [Fact]
    public async void GetSavedMail_WhenCalled_ReturnsNotFoundResult()
    {
        _unitOfWork.Setup(u => u.Mails.GetItem(It.IsAny<int>())).ReturnsAsync(() => null);
        _mapper.Setup(m => m.Map<MailDto>(It.IsAny<Mail>())).Returns(new Mock<MailDto>().Object);

        var notFound = await _mailController.GetSavedMail(It.IsAny<int>());

        Assert.IsType<NotFoundResult>(notFound);
    }
}
