using FluentValidation;

namespace MusicStore.Application.Inventory.Commands.PurchaseInventoryItems;

public class PurchaseInventoryItemsCommandValidator : AbstractValidator<PurchaseInventoryItemsCommand>
{
    public PurchaseInventoryItemsCommandValidator()
    {
        RuleFor(x => x.InventoryItemsToPurchase)
            .NotEmpty().WithMessage("InventoryItemsToPurchase is required.");
    }
}
