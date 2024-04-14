using FluentAssertions;
using MusicStore.Application.Inventory.Commands.PurchaseInventoryItems;

namespace MusicStore.Application.UnitTests.Inventory.Commands.PurchaseInventoryItems;

[TestFixture]
public class PurchaseInventoryItemsCommandValidatorTests
{
    private PurchaseInventoryItemsCommandValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new PurchaseInventoryItemsCommandValidator();
    }

    [Test]
    public void GivenPurchaseInventoryItemsCommandValidator_WhenValidatingValidCommand_ThenNoErrors()
    {
        // arrange
        var command = new PurchaseInventoryItemsCommand
        {
            InventoryItemsToPurchase = new Dictionary<int, int>
            {
                { 1, 10 }
            }
        };

        // act
        var result = _validator.Validate(command);

        // assert
        result.Errors.Count.Should().Be(0);
    }

    [Test]
    public void InventoryItemsToPurchase_Empty_IsInvalid()
    {
        // arrange
        var command = new PurchaseInventoryItemsCommand
        {
            InventoryItemsToPurchase = new Dictionary<int, int>()
        };

        // act
        var result = _validator.Validate(command);

        // assert
        result.Errors.Count.Should().Be(1);
        result.Errors.First().ErrorMessage.Should().Be("InventoryItemsToPurchase is required.");
    }

    [Test]
    public void InventoryItemsToPurchase_Null_IsInvalid()
    {
        // arrange
        var command = new PurchaseInventoryItemsCommand();

        // act
        var result = _validator.Validate(command);

        // assert
        // assert
        result.Errors.Count.Should().Be(1);
        result.Errors.First().ErrorMessage.Should().Be("InventoryItemsToPurchase is required.");
    }
}