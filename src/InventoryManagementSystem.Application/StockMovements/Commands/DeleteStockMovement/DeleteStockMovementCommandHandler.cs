using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;

namespace InventoryManagementSystem.Application.StockMovements.Commands.DeleteStockMovement;

public class DeleteStockMovementCommandHandler(IStockMovementRepository stockMovementRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteStockMovementCommand, bool>
{
    public async Task<bool> Handle(DeleteStockMovementCommand request, CancellationToken cancellationToken)
    {
        var movement = await stockMovementRepository.GetByIdAsync(request.Id, cancellationToken);
        if (movement is null)
        {
            return false;
        }

        await stockMovementRepository.DeleteAsync(movement, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
