using InventoryManagementSystem.Domain.Enums;

namespace InventoryManagementSystem.Api.Contracts.StockMovements;

public sealed record CreateStockMovementRequest(
    Guid ProductId,
    Guid WarehouseId,
    StockMovementType Type,
    int Quantity,
    Guid? PerformedByUserId,
    string? Notes);
