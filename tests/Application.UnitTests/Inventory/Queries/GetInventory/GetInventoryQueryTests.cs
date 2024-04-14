using AutoMapper;
using MusicStore.Application.Inventory.Queries.GetInventory;
using FluentAssertions;
using Moq;
using MusicStore.Application.Common.Interfaces;

namespace MusicStore.Application.UnitTests.Inventory.Queries.GetInventory;

public class Tests
{
    private Mock<IApplicationDbContext> _dbContext = null!;
    private Mock<IMapper> _mapper = null!;

    [SetUp]
    public void Setup()
    {
        _dbContext = new Mock<IApplicationDbContext>();
        _mapper = new Mock<IMapper>();
    }

    [Test]
    public async Task GivenGetInventoryQueryHandler_WhenQueryIsHandled_ThenReturnTypeIsCorrect()
    {
        // arrange
        var handler = new GetInventoryQueryHandler(_dbContext.Object, _mapper.Object);

        // act
        var result = await handler.Handle(new GetInventoryQuery(), new CancellationToken());

        // assert
        result.Should().BeOfType<List<InventoryItem>>();
    }
}
