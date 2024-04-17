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

    [SetUp]
    public void Setup()
    {

        var entities = new List<Domain.Entities.InventoryItem>
        {
            new()
            {
                Id = 1, Title = "Abbey Road", Artist = "The Beatles", Year = 1969, Genre = "Rock",
                Price = 12.99m, StockCount = 100
            }
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
