using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;

namespace InventoryManagementSystem.Application.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductByIdQuery, ProductResponse?>
{
    public async Task<ProductResponse?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
            return null;

        if (!product.IsActive && !request.IncludeInactive)
            return null;

        return ProductResponse.FromEntity(product);
    }
}
