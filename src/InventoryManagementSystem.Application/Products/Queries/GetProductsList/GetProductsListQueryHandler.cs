using System.Linq;
using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Application.Abstractions.Pagination;
using InventoryManagementSystem.Application.Products.Queries.GetProductById;

namespace InventoryManagementSystem.Application.Products.Queries.GetProductsList;

public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, PagedResult<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsListQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PagedResult<ProductResponse>> Handle(
        GetProductsListQuery request,
        CancellationToken cancellationToken)
    {
        var (products, totalCount) = await _productRepository.GetPagedAsync(
            request.Filter,
            request.Pagination,
            cancellationToken);

        var items = products.Select(ProductResponse.FromEntity)
            .ToList();

        return new PagedResult<ProductResponse>(
            items,
            request.Pagination.Page,
            request.Pagination.PageSize,
            totalCount);
    }
}
