using MediatR;
using MusicStore.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Application.Common.Exceptions;

namespace MusicStore.Application.Inventory.Commands;

public record PurchaseInventoryItemsCommand : IRequest
{
    public Dictionary<int, int> InventoryItemsToPurchase { get; init; }
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

            entity.StockCount -= inventoryItemToPurchase.Value;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
