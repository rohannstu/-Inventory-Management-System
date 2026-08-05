using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;

namespace InventoryManagementSystem.Application.Warehouses.Queries.GetWarehouseById;

public class GetWarehouseByIdQueryHandler(IWarehouseRepository warehouseRepository)
    : IRequestHandler<GetWarehouseByIdQuery, WarehouseResponse?>
{
    public async Task<WarehouseResponse?> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
    {
        var warehouse = await warehouseRepository.GetByIdAsync(request.Id, cancellationToken);
        return warehouse is null
            ? null
            : new WarehouseResponse(warehouse.Id, warehouse.Name, warehouse.Location, warehouse.IsActive);
    }
}
