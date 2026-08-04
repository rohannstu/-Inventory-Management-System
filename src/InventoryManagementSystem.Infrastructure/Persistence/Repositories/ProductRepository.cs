using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Application.Abstractions.Pagination;
using InventoryManagementSystem.Application.Products.Queries.GetProductsList;
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

    public async Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
        ProductListFilter filter,
        PaginationParams pagination,
        CancellationToken cancellationToken)
    {
        IQueryable<Product> query = dbContext.Products;

        if (filter.CategoryId.HasValue)
            query = query.Where(p => p.CategoryId == filter.CategoryId.Value);

        if (filter.SupplierId.HasValue)
            query = query.Where(p => p.SupplierId == filter.SupplierId.Value);

        if (filter.IsActive.HasValue)
            query = query.Where(p => p.IsActive == filter.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            query = query.Where(p => EF.Functions.ILike(p.Name, $"%{filter.SearchTerm}%"));

        var totalCount = await query.CountAsync(cancellationToken);

        query = pagination.SortBy?.ToLowerInvariant() switch
        {
            "name" => pagination.SortDescending
                ? query.OrderByDescending(p => p.Name)
                : query.OrderBy(p => p.Name),
            "price" => pagination.SortDescending
                ? query.OrderByDescending(p => p.Price.Amount)
                : query.OrderBy(p => p.Price.Amount),
            "stockquantity" => pagination.SortDescending
                ? query.OrderByDescending(p => p.StockQuantity)
                : query.OrderBy(p => p.StockQuantity),
            _ => query.OrderBy(p => p.Name)
        };

        var items = await query
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}