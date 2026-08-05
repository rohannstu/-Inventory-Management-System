using InventoryManagementSystem.Domain.Enums;

namespace InventoryManagementSystem.Api.Contracts.StockMovements;

public sealed record StockMovementResponse(
    Guid Id,
    Guid ProductId,
    Guid WarehouseId,
    StockMovementType Type,
    int Quantity,
    string? Notes,
    DateTimeOffset OccurredAtUtc,
    Guid? PerformedByUserId);
