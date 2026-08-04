using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Pagination;
using InventoryManagementSystem.Application.Products.Queries.GetProductById;

namespace InventoryManagementSystem.Application.Products.Queries.GetProductsList;

public record GetProductsListQuery(
    ProductListFilter Filter,
    PaginationParams Pagination)
    : IQuery<PagedResult<ProductResponse>>;
