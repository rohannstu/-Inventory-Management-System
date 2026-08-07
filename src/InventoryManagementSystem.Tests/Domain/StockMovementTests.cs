using FluentAssertions;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.Enums;
using Xunit;

namespace InventoryManagementSystem.Tests.Domain;

public class StockMovementTests
{
    [Fact]
    public void CreateStockIn_Should_Set_Type_To_StockIn()
    {
        var movement = StockMovement.CreateStockIn(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 10, null);

        movement.Type.Should().Be(StockMovementType.StockIn);
        movement.Quantity.Should().Be(10);
    }

    [Fact]
    public void CreateStockOut_Should_Set_Type_To_StockOut()
    {
        var movement = StockMovement.CreateStockOut(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 5, null);

        movement.Type.Should().Be(StockMovementType.StockOut);
        movement.Quantity.Should().Be(5);
    }

    [Fact]
    public void CreateAdjustment_Should_Set_Type_To_Adjustment()
    {
        var movement = StockMovement.CreateAdjustment(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 3, null);

        movement.Type.Should().Be(StockMovementType.Adjustment);
        movement.Quantity.Should().Be(3);
    }

    [Fact]
    public void Constructor_Should_Set_All_Properties()
    {
        var productId = Guid.NewGuid();
        var warehouseId = Guid.NewGuid();
        var performedByUserId = Guid.NewGuid();
        var movement = StockMovement.CreateStockIn(Guid.NewGuid(), productId, warehouseId, 10, performedByUserId, "Test notes");

        movement.ProductId.Should().Be(productId);
        movement.WarehouseId.Should().Be(warehouseId);
        movement.Type.Should().Be(StockMovementType.StockIn);
        movement.Quantity.Should().Be(10);
        movement.Notes.Should().Be("Test notes");
        movement.PerformedByUserId.Should().Be(performedByUserId);
    }

    [Fact]
    public void Constructor_Should_Set_OccurredAtUtc_To_Now()
    {
        var before = DateTimeOffset.UtcNow.AddSeconds(-1);
        var movement = StockMovement.CreateStockIn(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1, null);
        var after = DateTimeOffset.UtcNow.AddSeconds(1);

        movement.OccurredAtUtc.Should().BeAfter(before);
        movement.OccurredAtUtc.Should().BeBefore(after);
    }

    [Fact]
    public void Constructor_Should_Throw_When_Quantity_Is_Not_Positive()
    {
        Action act = () => StockMovement.CreateStockIn(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 0, null);

        act.Should().Throw<ArgumentException>().WithMessage("Quantity must be positive.*");
    }

    [Fact]
    public void Constructor_Should_Throw_When_Quantity_Is_Negative()
    {
        Action act = () => StockMovement.CreateStockIn(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), -1, null);

        act.Should().Throw<ArgumentException>().WithMessage("Quantity must be positive.*");
    }
}