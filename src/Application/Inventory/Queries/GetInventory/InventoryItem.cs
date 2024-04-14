﻿using MusicStore.Application.Common.Mappings;

namespace MusicStore.Application.Inventory.Queries.GetInventory;

public class InventoryItem : IMapFrom<Domain.Entities.InventoryItem>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public int Year { get; set; }
    public string? Genre { get; set; }
    public decimal Price { get; set; }
}
