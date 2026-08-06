using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Infrastructure.Persistence.Repositories;

public class WarehouseRepository(AppDbContext dbContext) : IWarehouseRepository
{
    public Task<Warehouse?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => dbContext.Warehouses.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

    public async Task AddAsync(Warehouse warehouse, CancellationToken cancellationToken)
        => await dbContext.Warehouses.AddAsync(warehouse, cancellationToken);

    public Task UpdateAsync(Warehouse warehouse, CancellationToken cancellationToken)
    {
        dbContext.Warehouses.Update(warehouse);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Warehouse warehouse, CancellationToken cancellationToken)
    {
        dbContext.Warehouses.Remove(warehouse);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        => dbContext.Warehouses.AnyAsync(w => w.Name == name, cancellationToken);
}
