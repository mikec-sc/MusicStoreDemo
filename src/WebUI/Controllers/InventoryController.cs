using Application.Inventory.Queries.GetInventory;
using Microsoft.AspNetCore.Mvc;

namespace MusicStore.WebUI.Controllers;

public class InventoryController : ApiControllerBase
{
    [HttpGet]
    public IEnumerable<InventoryItem> Get()
    {
        return new List<InventoryItem>
        {
            new() { Id = Guid.NewGuid(), Title = "Abbey Road", Artist = "The Beatles", Year = 1969, Genre = "Rock", Price = 12.99m },
            new() { Id = Guid.NewGuid(), Title = "Thriller", Artist = "Michael Jackson", Year = 1982, Genre = "Pop", Price = 14.99m },
            new() { Id = Guid.NewGuid(), Title = "Dark Side of the Moon", Artist = "Pink Floyd", Year = 1973, Genre = "Progressive Rock", Price = 11.99m },
            new() { Id = Guid.NewGuid(), Title = "Back in Black", Artist = "AC/DC", Year = 1980, Genre = "Rock", Price = 10.99m },
            new() { Id = Guid.NewGuid(), Title = "Rumours", Artist = "Fleetwood Mac", Year = 1977, Genre = "Rock", Price = 13.49m }
        };
    }
}