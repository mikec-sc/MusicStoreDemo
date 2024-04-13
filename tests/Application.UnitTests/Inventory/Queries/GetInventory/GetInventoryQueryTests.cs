using MusicStore.Application.Inventory.Queries.GetInventory;
using FluentAssertions;

namespace MusicStore.Application.UnitTests.Inventory.Queries.GetInventory;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GivenGetInventoryQueryHandler_WhenQueryIsHandled_ThenReturnTypeIsCorrect()
    {
        // arrange
        var handler = new GetInventoryQueryHandler();

        // act
        var result = await handler.Handle(new GetInventoryQuery(), new CancellationToken());

        // assert
        result.Should().BeOfType<List<InventoryItem>>();
    }
}
