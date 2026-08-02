using InventoryManagementSystem.Domain.Common;
using InventoryManagementSystem.Domain.Enums;

namespace InventoryManagementSystem.Domain.Entities;

public class StockMovement : Entity<Guid>
{
    public Guid ProductId { get; private set; }
    public Product? Product { get; private set; }

    public Guid WarehouseId { get; private set; }
    public Warehouse? Warehouse { get; private set; }

    public StockMovementType Type { get; private set; }
    public int Quantity { get; private set; }
    public string? Notes { get; private set; }
    public DateTimeOffset OccurredAtUtc { get; private set; }

    // Deferred reference — no User entity exists yet; Identity owns that in Phase 7.
    public Guid? PerformedByUserId { get; private set; }

    private StockMovement() { } // EF Core

    private StockMovement(
        Guid id,
        Guid productId,
        Guid warehouseId,
        StockMovementType type,
        int quantity,
        Guid? performedByUserId,
        string? notes)
        : base(id)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.", nameof(quantity));

        ProductId = productId;
        WarehouseId = warehouseId;
        Type = type;
        Quantity = quantity;
        PerformedByUserId = performedByUserId;
        Notes = notes;
        OccurredAtUtc = DateTimeOffset.UtcNow;
    }

    // Factory methods instead of a public constructor
    public static StockMovement CreateStockIn(
        Guid id, Guid productId, Guid warehouseId, int quantity, Guid? performedByUserId, string? notes = null)
        => new(id, productId, warehouseId, StockMovementType.StockIn, quantity, performedByUserId, notes);

    public static StockMovement CreateStockOut(
        Guid id, Guid productId, Guid warehouseId, int quantity, Guid? performedByUserId, string? notes = null)
        => new(id, productId, warehouseId, StockMovementType.StockOut, quantity, performedByUserId, notes);

    public static StockMovement CreateAdjustment(
        Guid id, Guid productId, Guid warehouseId, int quantity, Guid? performedByUserId, string? notes = null)
        => new(id, productId, warehouseId, StockMovementType.Adjustment, quantity, performedByUserId, notes);
}