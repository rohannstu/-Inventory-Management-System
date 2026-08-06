using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.Warehouses.Commands.CreateWarehouse;

public class CreateWarehouseCommandHandler(
    IWarehouseRepository warehouseRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateWarehouseCommand, Guid>
{
    public async Task<Guid> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        if (await warehouseRepository.ExistsByNameAsync(request.Name, cancellationToken))
            throw new InvalidOperationException($"A warehouse with name '{request.Name}' already exists.");

        var warehouse = new Warehouse(
            id: Guid.NewGuid(),
            name: request.Name,
            location: request.Location);

        await warehouseRepository.AddAsync(warehouse, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return warehouse.Id;
    }
}
