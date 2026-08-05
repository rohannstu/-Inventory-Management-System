using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Pagination;
using InventoryManagementSystem.Application.StockMovements.Queries.GetStockMovementById;

namespace InventoryManagementSystem.Application.StockMovements.Queries.GetStockMovementsList;

public sealed record GetStockMovementsListQuery(
    StockMovementListFilter Filter,
    PaginationParams Pagination) : IQuery<PagedResult<StockMovementResponse>>;
