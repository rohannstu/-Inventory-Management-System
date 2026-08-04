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

        product.Rename(request.Name);
        product.UpdateDescription(request.Description);
        product.ChangePrice(Money.Create(request.Price, request.Currency));
        product.ChangeCategory(request.CategoryId);
        product.ChangeSupplier(request.SupplierId);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
