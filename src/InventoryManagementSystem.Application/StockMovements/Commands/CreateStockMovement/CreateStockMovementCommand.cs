using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Domain.Enums;

namespace InventoryManagementSystem.Application.StockMovements.Commands.CreateStockMovement;

public sealed record CreateStockMovementCommand(
    Guid ProductId,
    Guid WarehouseId,
    StockMovementType Type,
    int Quantity,
    Guid? PerformedByUserId,
    string? Notes) : ICommand<Guid>;
