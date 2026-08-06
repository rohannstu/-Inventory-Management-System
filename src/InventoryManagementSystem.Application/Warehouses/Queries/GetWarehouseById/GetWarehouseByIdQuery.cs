using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Warehouses.Queries.GetWarehouseById;

public sealed record GetWarehouseByIdQuery(Guid Id) : IQuery<WarehouseResponse?>;
