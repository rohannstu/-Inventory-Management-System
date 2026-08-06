using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;

namespace InventoryManagementSystem.Application.StockMovements.Queries.GetStockMovementById;

public class GetStockMovementByIdQueryHandler(IStockMovementRepository stockMovementRepository)
    : IRequestHandler<GetStockMovementByIdQuery, StockMovementResponse?>
{
    public async Task<StockMovementResponse?> Handle(GetStockMovementByIdQuery request, CancellationToken cancellationToken)
    {
        var movement = await stockMovementRepository.GetByIdAsync(request.Id, cancellationToken);
        return movement is null
            ? null
            : new StockMovementResponse(
                movement.Id,
                movement.ProductId,
                movement.WarehouseId,
                movement.Type,
                movement.Quantity,
                movement.Notes,
                movement.OccurredAtUtc,
                movement.PerformedByUserId);
    }
}
