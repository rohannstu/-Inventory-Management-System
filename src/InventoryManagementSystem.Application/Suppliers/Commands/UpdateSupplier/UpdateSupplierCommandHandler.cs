using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;

namespace InventoryManagementSystem.Application.Suppliers.Commands.UpdateSupplier;

public class UpdateSupplierCommandHandler(
    ISupplierRepository supplierRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateSupplierCommand, bool>
{
    public async Task<bool> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepository.GetByIdAsync(request.Id, cancellationToken);
        if (supplier is null)
            return false;

        if (!string.Equals(supplier.Name, request.Name, StringComparison.OrdinalIgnoreCase)
            && await supplierRepository.ExistsByNameAsync(request.Name, cancellationToken))
        {
            throw new InvalidOperationException($"A supplier with name '{request.Name}' already exists.");
        }

        supplier.Rename(request.Name);
        supplier.UpdateContactInfo(request.ContactEmail, request.ContactPhone);

        await supplierRepository.UpdateAsync(supplier, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
