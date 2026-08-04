using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Infrastructure.Persistence.Repositories;

public class ProductRepository(AppDbContext dbContext) : IProductRepository
{
    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => dbContext.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public Task<bool> SkuExistsAsync(string sku, CancellationToken cancellationToken)
    {
        var skuToCheck = Sku.Create(sku);
        return dbContext.Products.AnyAsync(p => p.Sku == skuToCheck, cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken)
        => await dbContext.Products.AddAsync(product, cancellationToken);
}