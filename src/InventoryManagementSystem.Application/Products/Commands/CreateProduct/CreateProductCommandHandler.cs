using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.ValueObjects;

namespace InventoryManagementSystem.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var skuAlreadyExists = await productRepository.SkuExistsAsync(request.Sku, cancellationToken);
        if (skuAlreadyExists)
            throw new InvalidOperationException($"A product with SKU '{request.Sku}' already exists.");

        var sku = Sku.Create(request.Sku);
        var price = Money.Create(request.Price, request.Currency);

        var product = new Product(
            id: Guid.NewGuid(),
            sku: sku,
            name: request.Name,
            price: price,
            categoryId: request.CategoryId,
            supplierId: request.SupplierId,
            description: request.Description);

        await productRepository.AddAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
