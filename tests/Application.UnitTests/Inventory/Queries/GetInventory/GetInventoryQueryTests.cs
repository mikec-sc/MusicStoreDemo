using AutoMapper;
using MusicStore.Application.Inventory.Queries.GetInventory;
using FluentAssertions;
using Moq;
using MusicStore.Application.Common.Interfaces;
using MockQueryable.Moq;
using MusicStore.Application.Common.Models;

namespace MusicStore.Application.UnitTests.Inventory.Queries.GetInventory;

public class GetInventoryQueryTests
{
    private Mock<IApplicationDbContext> _dbContext = null!;
    private Mock<IMapper> _mapper = null!;

    private readonly List<Domain.Entities.InventoryItem> _entities = new()
    {
        new Domain.Entities.InventoryItem
        {
            Id = 1,
            Title = "Abbey Road",
            Artist = "The Beatles",
            Year = 1969,
            Genre = "Rock",
            Price = 12.99m,
            StockCount = 100
        }
    };

    [SetUp]
    public void Setup()
    {
        _dbContext = new Mock<IApplicationDbContext>();
        _dbContext.Setup(x => x.InventoryItems)
            .Returns(_entities.AsQueryable().BuildMockDbSet().Object);

        _mapper = new Mock<IMapper>();
        _mapper.Setup(x => x.ConfigurationProvider)
            .Returns(
                () => new MapperConfiguration(
                    cfg => { cfg.CreateMap<Domain.Entities.InventoryItem, InventoryItem>(); }));
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

    [Test]
    public async Task GivenGetInventoryQueryHandler_WhenQueryIsHandled_ThenReturnValueIsCorrect()
    {
        // arrange
        var handler = new GetInventoryQueryHandler(_dbContext.Object, _mapper.Object);

        // act
        var result = await handler.Handle(new GetInventoryQuery(), new CancellationToken());

        // assert
        result.Should().BeEquivalentTo(_entities);
    }
}
