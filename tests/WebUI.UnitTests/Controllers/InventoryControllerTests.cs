using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MusicStore.Application.Common.Models;
using MusicStore.Application.Inventory.Queries.GetInventory;
using MusicStore.WebUI.Controllers;
using MusicStore.Application.Inventory.Commands.PurchaseInventoryItems;

namespace MusicStore.WebUI.UnitTests.Controllers;

public class InventoryControllerTests
{
    private InventoryController _controller = null!;
    private Mock<IMediator> _mediator = null!;

    [SetUp]
    public void Setup()
    {
        _controller = new InventoryController();
        _mediator = new Mock<IMediator>();

        var httpContext = new Mock<HttpContext>();
        httpContext.Setup(x => x.RequestServices.GetService(typeof(ISender)))
            .Returns(_mediator.Object);
        _controller.ControllerContext.HttpContext = httpContext.Object;
    }

    [Test]
    public async Task GivenInventoryController_WhenGetInventoryIsCalledWithNoId_ThenReturnTypeIsCorrect()
    {
        // arrange
        
        // act
        var result = (await _controller.Get()).ToList();

        // assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IEnumerable<InventoryItem>>();
    }

    [Test]
    public async Task GivenInventoryController_WhenGetInventoryIsCalledWithNoId_ThenCorrectQueryUsedForMediator()
    {
        // arrange

        // act
        await _controller.Get();

        // assert
        _mediator.Verify(x => x.Send(It.IsAny<GetInventoryQuery>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task GivenInventoryController_WhenGetInventoryIsCalledWithId_ThenCorrectQueryUsedForMediator()
    {
        // arrange
        const int id = 1;

        // act
        await _controller.Get(id);

        // assert
        _mediator.Verify(x => x.Send(It.Is<GetInventoryItemQuery>(y => y.Id == id), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task GivenInventoryController_WhenGetInventoryIsCalledWithId_ThenReturnTypeIsCorrect()
    {
        // arrange
        const int id = 1;
        _mediator.Setup(x => x.Send(It.Is<GetInventoryItemQuery>(x => x.Id == 1), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new InventoryItem()));

        // act
        var result = await _controller.Get(id);

        // assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<InventoryItem>();
    }

    [Test]
    public async Task GivenInventoryController_WhenPurchaseInventoryItemsIsCalled_ThenReturnTypeIsCorrect()
    {
        // arrange
        var request = new PurchaseInventoryItemsCommand();

        // act
        var result = await _controller.PurchaseInventoryItems(request);

        // assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ActionResult>();
    }

    [Test]
    public async Task GivenInventoryController_WhenPurchaseInventoryItemsIsCalled_ThenCorrectQueryUsedForMediator()
    {
        PurchaseInventoryItemsCommand? actualRequest = null;

        // arrange
        var request = new PurchaseInventoryItemsCommand
        {
            InventoryItemsToPurchase = new Dictionary<int, int>
            {
                { 1, 2 },
                { 3, 4 }
            }
        };
        _mediator.Setup(x => x.Send(request, It.IsAny<CancellationToken>()))
            .Callback<PurchaseInventoryItemsCommand, CancellationToken>((y, _) => actualRequest = y);

        // act
        await _controller.PurchaseInventoryItems(request);

        // assert
        _mediator.Verify(x => x.Send(It.IsAny<PurchaseInventoryItemsCommand>(), It.IsAny<CancellationToken>()), Times.Once());
        actualRequest.Should().BeEquivalentTo(request);
    }
}
