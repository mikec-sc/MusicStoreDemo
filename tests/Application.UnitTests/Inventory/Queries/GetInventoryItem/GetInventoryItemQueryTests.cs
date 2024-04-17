using AutoMapper;
using MusicStore.Application.Inventory.Queries.GetInventory;
using FluentAssertions;
using Moq;
using MusicStore.Application.Common.Interfaces;
using MockQueryable.Moq;
using MusicStore.Application.Common.Models;

namespace MusicStore.Application.UnitTests.Inventory.Queries.GetInventoryItem;

public class GetInventoryItemQueryTests
{
    private Mock<IApplicationDbContext> _dbContext = null!;
    private Mock<IMapper> _mapper = null!;

    private readonly Domain.Entities.InventoryItem _album = new()
    {
        Id = 1, Title = "Abbey Road", Artist = "The Beatles", Year = 1969, Genre = "Rock",
        Price = 12.99m, StockCount = 100
    };

    [SetUp]
    public void Setup()
    {
        var entities = new List<Domain.Entities.InventoryItem>
        {
            _album
        };
        _dbContext = new Mock<IApplicationDbContext>();
        _dbContext.Setup(x => x.InventoryItems)
            .Returns(entities.AsQueryable().BuildMockDbSet().Object);

        _mapper = new Mock<IMapper>();
        _mapper.Setup(x => x.ConfigurationProvider)
            .Returns(
                () => new MapperConfiguration(
                    cfg => { cfg.CreateMap<Domain.Entities.InventoryItem, InventoryItem>(); }));
    }

    [Test]
    public async Task GivenGetInventoryItemQueryHandler_WhenQueryIsHandled_ThenReturnTypeIsCorrect()
    {
        // arrange
        var handler = new GetInventoryItemQueryHandler(_dbContext.Object, _mapper.Object);

        // act
        var result = await handler.Handle(new GetInventoryItemQuery { Id = 1 }, new CancellationToken());

        // assert
        result.Should().BeOfType<InventoryItem>();
    }

    [Test]
    public async Task GivenGetInventoryItemQueryHandler_WhenQueryIsHandled_ThenReturnValueIsCorrect()
    {
        // arrange
        var handler = new GetInventoryItemQueryHandler(_dbContext.Object, _mapper.Object);

        // act
        var result = await handler.Handle(new GetInventoryItemQuery { Id = _album.Id }, new CancellationToken());

        // assert
        result.Should().BeEquivalentTo(_album);
    }
}
