using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Suppliers.Commands.CreateSupplier;

public class CreateSupplierCommandHandler(
    ISupplierRepository supplierRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSupplierCommand, Guid>
{
    public async Task<Guid> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        if (await supplierRepository.ExistsByNameAsync(request.Name, cancellationToken))
            throw new InvalidOperationException($"A supplier with name '{request.Name}' already exists.");

        var supplier = new Supplier(
            id: Guid.NewGuid(),
            name: request.Name,
            contactEmail: request.ContactEmail,
            contactPhone: request.ContactPhone);

        await supplierRepository.AddAsync(supplier, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return supplier.Id;
    }
}
