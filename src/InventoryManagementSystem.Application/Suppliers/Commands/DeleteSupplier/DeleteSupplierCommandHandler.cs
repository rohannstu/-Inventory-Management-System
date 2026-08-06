using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;

namespace InventoryManagementSystem.Application.Suppliers.Commands.DeleteSupplier;

public class DeleteSupplierCommandHandler(
    ISupplierRepository supplierRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteSupplierCommand, bool>
{
    public async Task<bool> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepository.GetByIdAsync(request.Id, cancellationToken);
        if (supplier is null)
            return false;

        await supplierRepository.DeleteAsync(supplier, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
