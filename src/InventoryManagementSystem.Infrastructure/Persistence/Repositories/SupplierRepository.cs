using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Infrastructure.Persistence.Repositories;

public class SupplierRepository(AppDbContext dbContext) : ISupplierRepository
{
    public Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => dbContext.Suppliers.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken)
        => await dbContext.Suppliers.AddAsync(supplier, cancellationToken);

    public Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken)
    {
        dbContext.Suppliers.Update(supplier);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Supplier supplier, CancellationToken cancellationToken)
    {
        dbContext.Suppliers.Remove(supplier);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        => dbContext.Suppliers.AnyAsync(s => s.Name == name, cancellationToken);
}
