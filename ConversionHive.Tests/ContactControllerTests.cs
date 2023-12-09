using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ConversionHive.Controllers;
using ConversionHive.Dtos.ContactDto;
using ConversionHive.Services;

namespace ConversionHive.Tests;

public class ContactControllerTests
{
    private readonly Mock<IContactService> _contactServices;
    private readonly ContactController _contactController;

    public ContactControllerTests()
    {
        _contactServices = new Mock<IContactService>();
        _contactController = new ContactController(_contactServices.Object);
    }

    [Fact]
    public async Task PostContact_WhenCalled_ReturnsCreatedAtAction()
    {
        _contactServices
            .Setup(s => s.PostContact(It.IsAny<CreateContactDto>()))
            .ReturnsAsync(new Mock<ReadContactDto>().Object);

        var result = await _contactController.PostContact(new Mock<CreateContactDto>().Object);

        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task PostContact_WhenCalled_ReturnsBadRequest()
    {
        var result = await _contactController.PostContact(null);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task GetContact_WhenCalled_ReturnsOkResult()
    {
        _contactServices.Setup(s => s.GetContact(It.IsAny<int>())).ReturnsAsync(new Mock<ReadContactDto>().Object);

        var result = await _contactController.GetContact(It.IsAny<int>());

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetContact_WhenCalled_ReturnsNotFoundResult()
    {
        _contactServices.Setup(c => c.GetContact(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        var result = await _contactController.GetContact(It.IsAny<int>());

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task PostMultipleContacts_WhenCalled_ReturnsOkResult()
    {
        var formFile = new Mock<IFormFile>();
        var authorization = "my authorization string";

        _contactServices.Setup(c => c.ProcessContacts(It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync(new Mock<IEnumerable<CreateContactDto>>().Object);

        var result = await _contactController
            .PostMultipleContacts(authorization, formFile.Object);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task PostMultipleContacts_WhenCalled_ReturnsBadRequest()
    {
        var formFile = new Mock<IFormFile>();
        var authorization = "my authorization string";

        _contactServices.Setup(c => c.ProcessContacts(It.IsAny<string>(), It.IsAny<Stream>()))
            .ReturnsAsync(() => null);

        var result = await _contactController
            .PostMultipleContacts(authorization, formFile.Object);

        Assert.IsType<BadRequestResult>(result);
    }
}
