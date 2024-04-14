using Microsoft.AspNetCore.Mvc;
using MusicStore.Application.Inventory.Commands;
using MusicStore.Application.Inventory.Queries.GetInventory;

namespace MusicStore.WebUI.Controllers;

public class InventoryController : ApiControllerBase
{
    /// <summary>
    /// Retrieves a list of all InventoryItems from the database.
    /// </summary>
    /// <returns>A collection of InventoryItem objects representing the inventory items stored in the database.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<InventoryItem>> Get()
    {
        return await Mediator.Send(new GetInventoryQuery());
    }

    /// <summary>
    /// Update InventoryItems StockLevel in DB
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> PurchaseInventoryItems(PurchaseInventoryItemsCommand command)
    {
        await Mediator.Send(command);

        return Ok();
    }
}
