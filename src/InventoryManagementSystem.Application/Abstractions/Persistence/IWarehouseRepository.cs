using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Abstractions.Persistence;

public interface IWarehouseRepository
{
    Task<Warehouse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Warehouse warehouse, CancellationToken cancellationToken);
    Task UpdateAsync(Warehouse warehouse, CancellationToken cancellationToken);
    Task DeleteAsync(Warehouse warehouse, CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
}
