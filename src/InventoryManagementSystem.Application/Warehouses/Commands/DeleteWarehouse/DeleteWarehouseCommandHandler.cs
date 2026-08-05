using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;

namespace InventoryManagementSystem.Application.Warehouses.Commands.DeleteWarehouse;

public class DeleteWarehouseCommandHandler(
    IWarehouseRepository warehouseRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteWarehouseCommand, bool>
{
    public async Task<bool> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = await warehouseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (warehouse is null)
            return false;

        await warehouseRepository.DeleteAsync(warehouse, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
