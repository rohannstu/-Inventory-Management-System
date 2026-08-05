using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Abstractions.Persistence;

public interface IStockMovementRepository
{
    Task<StockMovement?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(StockMovement movement, CancellationToken cancellationToken);
    Task DeleteAsync(StockMovement movement, CancellationToken cancellationToken);
}
