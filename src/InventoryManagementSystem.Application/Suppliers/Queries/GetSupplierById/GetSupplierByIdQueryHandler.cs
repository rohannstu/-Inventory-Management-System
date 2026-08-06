using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;

namespace InventoryManagementSystem.Application.Suppliers.Queries.GetSupplierById;

public class GetSupplierByIdQueryHandler(ISupplierRepository supplierRepository)
    : IRequestHandler<GetSupplierByIdQuery, SupplierResponse?>
{
    public async Task<SupplierResponse?> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        var supplier = await supplierRepository.GetByIdAsync(request.Id, cancellationToken);
        return supplier is null
            ? null
            : new SupplierResponse(supplier.Id, supplier.Name, supplier.ContactEmail, supplier.ContactPhone, supplier.IsActive);
    }
}
