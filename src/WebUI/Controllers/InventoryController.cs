using Microsoft.AspNetCore.Mvc;
using MusicStore.Application.Inventory.Queries.GetInventory;

namespace MusicStore.WebUI.Controllers;

public class InventoryController : ApiControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<InventoryItem>> Get()
    {
        return await Mediator.Send(new GetInventoryQuery());
    }
}
