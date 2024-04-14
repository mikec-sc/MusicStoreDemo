using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MusicStore.Application.Inventory.Queries.GetInventory;
using MusicStore.WebUI.Controllers;
using System.Net.Http;

namespace MusicStore.WebUI.UnitTests.Controllers;

public class InventoryControllerTests
{
    private readonly InventoryController _controller = new();

    private Mock<IMediator> _mediator = null!;

    [SetUp]
    public void Setup()
    {
        _mediator = new Mock<IMediator>();

        var httpContext = new Mock<HttpContext>();
        httpContext.Setup(x => x.RequestServices.GetService(typeof(ISender)))
            .Returns(_mediator.Object);
        _controller.ControllerContext.HttpContext = httpContext.Object;
    }

    [Test]
    public async Task GivenInventoryController_WhenGetInventoryIsCalled_ThenReturnTypeIsCorrect()
    {
        // arrange
        
        // act
        var result = (await _controller.Get()).ToList();

        // assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IEnumerable<InventoryItem>>();
    }

    [Test]
    public async Task GivenInventoryController_WhenGetInventoryIsCalled_ThenCorrectQueryUsedForMediator()
    {
        // arrange

        // act
        var result = (await _controller.Get()).ToList();

        // assert
        _mediator.Verify(x => x.Send(It.IsAny<GetInventoryQuery>(), It.IsAny<CancellationToken>()), Times.Once());
    }
}
