using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Domain.Entities;

namespace InventoryManagementSystem.Application.StockMovements.Commands.CreateStockMovement;

public class CreateStockMovementCommandHandler(
    IStockMovementRepository stockMovementRepository,
    IProductRepository productRepository,
    IWarehouseRepository warehouseRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateStockMovementCommand, Guid>
{
    public async Task<Guid> Handle(CreateStockMovementCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            throw new InvalidOperationException($"Product with ID '{request.ProductId}' was not found.");

        var warehouse = await warehouseRepository.GetByIdAsync(request.WarehouseId, cancellationToken);
        if (warehouse is null)
            throw new InvalidOperationException($"Warehouse with ID '{request.WarehouseId}' was not found.");

        var movement = request.Type switch
        {
            Domain.Enums.StockMovementType.StockIn => StockMovement.CreateStockIn(
                id: Guid.NewGuid(),
                productId: request.ProductId,
                warehouseId: request.WarehouseId,
                quantity: request.Quantity,
                performedByUserId: request.PerformedByUserId,
                notes: request.Notes),
            Domain.Enums.StockMovementType.StockOut => StockMovement.CreateStockOut(
                id: Guid.NewGuid(),
                productId: request.ProductId,
                warehouseId: request.WarehouseId,
                quantity: request.Quantity,
                performedByUserId: request.PerformedByUserId,
                notes: request.Notes),
            Domain.Enums.StockMovementType.Adjustment => StockMovement.CreateAdjustment(
                id: Guid.NewGuid(),
                productId: request.ProductId,
                warehouseId: request.WarehouseId,
                quantity: request.Quantity,
                performedByUserId: request.PerformedByUserId,
                notes: request.Notes),
            _ => throw new InvalidOperationException("Unsupported stock movement type.")
        };

        await stockMovementRepository.AddAsync(movement, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return movement.Id;
    }
}
