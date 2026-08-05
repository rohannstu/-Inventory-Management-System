using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Suppliers.Queries.GetSupplierById;

public sealed record GetSupplierByIdQuery(Guid Id) : IQuery<SupplierResponse?>;
