using InventoryManagementSystem.Application.Abstractions.Pagination;
using InventoryManagementSystem.Application.StockMovements.Queries.GetStockMovementsList;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Abstractions.Persistence;

public interface IStockMovementRepository
{
    Task<StockMovement?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(StockMovement movement, CancellationToken cancellationToken);
    Task DeleteAsync(StockMovement movement, CancellationToken cancellationToken);
    Task<(IReadOnlyList<StockMovement> Items, int TotalCount)> GetPagedAsync(
        StockMovementListFilter filter,
        PaginationParams pagination,
        CancellationToken cancellationToken);
}
