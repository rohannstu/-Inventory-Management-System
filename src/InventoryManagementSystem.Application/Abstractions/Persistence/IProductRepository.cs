using InventoryManagementSystem.Application.Abstractions.Pagination;
using InventoryManagementSystem.Application.Products.Queries.GetProductsList;
using InventoryManagementSystem.Domain.Entities;
using System.Collections.Generic;

namespace InventoryManagementSystem.Application.Abstractions.Persistence;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> SkuExistsAsync(string sku, CancellationToken cancellationToken);
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
        ProductListFilter filter,
        PaginationParams pagination,
        CancellationToken cancellationToken);
}
