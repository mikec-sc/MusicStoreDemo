using AutoMapper;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using MusicStore.Application.Common.Exceptions;
using MusicStore.Application.Common.Interfaces;
using MusicStore.Application.Inventory.Commands.PurchaseInventoryItems;
using MusicStore.Application.Inventory.Queries.GetInventory;

namespace MusicStore.Application.UnitTests.Inventory.Commands.PurchaseInventoryItems;

public class PurchaseInventoryItemsCommandTests
{
    private Mock<IApplicationDbContext> _dbContext = null!;

    [SetUp]
    public void Setup()
    {
        var inventoryItem = new Domain.Entities.InventoryItem
        {
            Id = 1,
            Title = "Abbey Road",
            Artist = "The Beatles",
            Year = 1969,
            Genre = "Rock",
            Price = 12.99m,
            StockCount = 100
        };
        var entities = new List<Domain.Entities.InventoryItem>
        {
            inventoryItem
        };
        _dbContext = new Mock<IApplicationDbContext>();
        _dbContext.Setup(x => x.InventoryItems)
            .Returns(entities.AsQueryable().BuildMockDbSet().Object);
        _dbContext.Setup(x => x.InventoryItems.FindAsync(1, It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult(entities.SingleOrDefault(x => x.Id == 1)));
    }

    [Test]
    public async Task GivenPurchaseInventoryItemsCommandHandler_WhenCommandIsHandled_ThenDbIsUpdatedCorrectly()
    {
        // arrange
        var handler = new PurchaseInventoryItemsCommandHandler(_dbContext.Object);
        var command = new PurchaseInventoryItemsCommand
        {
            InventoryItemsToPurchase = new Dictionary<int, int>
            {
                { 1, 10 }
            }
        };

        // act
        await handler.Handle(command, new CancellationToken());

        // assert
        _dbContext.Object.InventoryItems.First().StockCount.Should().Be(90);
        _dbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GivenPurchaseInventoryItemsCommandHandler_WhenInvalidCommandIsHandled_ThenNotFoundExceptionThrown()
    {
        // arrange
        var handler = new PurchaseInventoryItemsCommandHandler(_dbContext.Object);
        var command = new PurchaseInventoryItemsCommand
        {
            InventoryItemsToPurchase = new Dictionary<int, int>
            {
                { 1000, 10 }
            }
        };

        // act & assert
        Func<Task> action = async () => await handler.Handle(command, new CancellationToken());
        await action.Should().ThrowAsync<NotFoundException>().WithMessage("Entity \"InventoryItem\" (1000) was not found."); ;
    }
}
