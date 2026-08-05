using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;

namespace InventoryManagementSystem.Application.Warehouses.Commands.UpdateWarehouse;

public class UpdateWarehouseCommandHandler(
    IWarehouseRepository warehouseRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateWarehouseCommand, bool>
{
    public async Task<bool> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await warehouseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (warehouse is null)
            return false;

        if (!string.Equals(warehouse.Name, request.Name, StringComparison.OrdinalIgnoreCase)
            && await warehouseRepository.ExistsByNameAsync(request.Name, cancellationToken))
        {
            throw new InvalidOperationException($"A warehouse with name '{request.Name}' already exists.");
        }

        warehouse.Rename(request.Name);
        warehouse.Relocate(request.Location);

        await warehouseRepository.UpdateAsync(warehouse, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
