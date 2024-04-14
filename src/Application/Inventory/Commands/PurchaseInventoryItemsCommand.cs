﻿using MediatR;
using MusicStore.Application.Common.Interfaces;
using MusicStore.Application.Common.Exceptions;

namespace MusicStore.Application.Inventory.Commands;

public record PurchaseInventoryItemsCommand : IRequest
{
    public Dictionary<int, int> InventoryItemsToPurchase { get; init; } = null!;
}
public class PurchaseInventoryItemsCommandHandler : IRequestHandler<PurchaseInventoryItemsCommand>
{
    private readonly IApplicationDbContext _context;

    public PurchaseInventoryItemsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(PurchaseInventoryItemsCommand request, CancellationToken cancellationToken)
    {
        foreach (var inventoryItemToPurchase in request.InventoryItemsToPurchase)
        {
            var entity = await _context.InventoryItems
                .FindAsync(new object[] { inventoryItemToPurchase.Key }, cancellationToken);

            if (entity == null)
                throw new NotFoundException(nameof(Domain.Entities.InventoryItem), inventoryItemToPurchase.Key);

            // TODO: this could be thread un-safe if 2 purchases happen at the same time, could be a dblock or bad logic
            if (entity.StockCount < inventoryItemToPurchase.Value)
                throw new InvalidOperationException($"{nameof(Domain.Entities.InventoryItem)} insufficient stock for {entity.Id}");

            entity.StockCount -= inventoryItemToPurchase.Value;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
