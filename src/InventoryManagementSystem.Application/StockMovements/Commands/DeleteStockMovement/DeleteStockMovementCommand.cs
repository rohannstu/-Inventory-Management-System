using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.StockMovements.Commands.DeleteStockMovement;

public sealed record DeleteStockMovementCommand(Guid Id) : ICommand<bool>;
