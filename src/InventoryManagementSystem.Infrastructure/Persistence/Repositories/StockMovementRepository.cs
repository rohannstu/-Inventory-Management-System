using InventoryManagementSystem.Application.Abstractions.Persistence;
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
}
