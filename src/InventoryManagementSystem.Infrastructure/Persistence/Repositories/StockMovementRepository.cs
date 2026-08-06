using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Application.Abstractions.Pagination;
using InventoryManagementSystem.Application.StockMovements.Queries.GetStockMovementsList;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Infrastructure.Persistence.Repositories;

public class StockMovementRepository(AppDbContext dbContext) : IStockMovementRepository
{
    public Task<StockMovement?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => dbContext.StockMovements.FirstOrDefaultAsync(sm => sm.Id == id, cancellationToken);

    public async Task AddAsync(StockMovement movement, CancellationToken cancellationToken)
        => await dbContext.StockMovements.AddAsync(movement, cancellationToken);

    public Task DeleteAsync(StockMovement movement, CancellationToken cancellationToken)
    {
        dbContext.StockMovements.Remove(movement);
        return Task.CompletedTask;
    }

    public async Task<(IReadOnlyList<StockMovement> Items, int TotalCount)> GetPagedAsync(
        StockMovementListFilter filter,
        PaginationParams pagination,
        CancellationToken cancellationToken)
    {
        IQueryable<StockMovement> query = dbContext.StockMovements;

        if (filter.ProductId.HasValue)
            query = query.Where(sm => sm.ProductId == filter.ProductId.Value);

        if (filter.WarehouseId.HasValue)
            query = query.Where(sm => sm.WarehouseId == filter.WarehouseId.Value);

        if (filter.Type.HasValue)
            query = query.Where(sm => sm.Type == filter.Type.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        query = string.IsNullOrWhiteSpace(pagination.SortBy)
            ? query.OrderByDescending(sm => sm.OccurredAtUtc)
            : pagination.SortBy.ToLowerInvariant() switch
            {
                "occurredatutc" => pagination.SortDescending
                    ? query.OrderByDescending(sm => sm.OccurredAtUtc)
                    : query.OrderBy(sm => sm.OccurredAtUtc),
                "quantity" => pagination.SortDescending
                    ? query.OrderByDescending(sm => sm.Quantity)
                    : query.OrderBy(sm => sm.Quantity),
                _ => query.OrderByDescending(sm => sm.OccurredAtUtc)
            };

        var items = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
