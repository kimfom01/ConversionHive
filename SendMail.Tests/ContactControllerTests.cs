using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SendMail.Controllers;
using SendMail.Models;
using SendMail.Repository;
using SendMail.Services;

namespace SendMail.Tests;

public class ContactControllerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IContactService> _contactServices;
    private readonly ContactController _contactController;

    public ContactControllerTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _mapper = new Mock<IMapper>();
        _contactServices = new Mock<IContactService>();
        _contactController = new ContactController(_unitOfWork.Object, _mapper.Object, _contactServices.Object);
    }

    [Fact]
    public async void PostContact_WhenCalled_ReturnsCreatedAtAction()
    {
        _mapper.Setup(m => m.Map<Contact>(It.IsAny<ContactDto>())).Returns(new Mock<Contact>().Object);
        _unitOfWork.Setup(u => u.Contacts.AddItem(It.IsAny<Contact>())).ReturnsAsync(new Mock<Contact>().Object);

        var result = await _contactController.PostContact(new Mock<ContactDto>().Object);

        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async void PostContact_WhenCalled_ReturnsBadRequest()
    {
        var result = await _contactController.PostContact(null);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async void GetContact_WhenCalled_ReturnsOkResult()
    {
        _unitOfWork.Setup(u => u.Contacts.GetItem(It.IsAny<int>())).ReturnsAsync(new Mock<Contact>().Object);
        _mapper.Setup(m => m.Map<ContactDto>(It.IsAny<ContactDto>())).Returns(new Mock<ContactDto>().Object);

        var result = await _contactController.GetContact(It.IsAny<int>());

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void GetContact_WhenCalled_ReturnsNotFoundResult()
    {
        _unitOfWork.Setup(u => u.Contacts.GetItem(It.IsAny<int>())).ReturnsAsync(() => null);

        var result = await _contactController.GetContact(It.IsAny<int>());

        Assert.IsType<NotFoundResult>(result);
    }
}
