using InventoryManagementSystem.Domain.Enums;

namespace InventoryManagementSystem.Application.StockMovements.Queries.GetStockMovementById;

public sealed record StockMovementResponse(
    Guid Id,
    Guid ProductId,
    Guid WarehouseId,
    StockMovementType Type,
    int Quantity,
    string? Notes,
    DateTimeOffset OccurredAtUtc,
    Guid? PerformedByUserId);
