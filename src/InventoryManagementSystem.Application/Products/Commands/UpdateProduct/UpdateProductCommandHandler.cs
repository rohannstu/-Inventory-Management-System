using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Domain.ValueObjects;

namespace InventoryManagementSystem.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProductCommand, bool>
{
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
            return false;

        product.UpdateDetails(
            name: request.Name,
            description: request.Description,
            price: Money.Create(request.Price, request.Currency),
            categoryId: request.CategoryId,
            supplierId: request.SupplierId);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
