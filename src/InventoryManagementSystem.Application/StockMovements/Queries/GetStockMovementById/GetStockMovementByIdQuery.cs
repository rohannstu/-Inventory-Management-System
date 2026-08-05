using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.StockMovements.Queries.GetStockMovementById;

public sealed record GetStockMovementByIdQuery(Guid Id) : IQuery<StockMovementResponse?>;
