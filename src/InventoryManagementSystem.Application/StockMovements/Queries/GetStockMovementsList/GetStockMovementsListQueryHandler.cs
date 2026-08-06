using System.Linq;
using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Application.Abstractions.Pagination;
using InventoryManagementSystem.Application.StockMovements.Queries.GetStockMovementById;

namespace InventoryManagementSystem.Application.StockMovements.Queries.GetStockMovementsList;

public class GetStockMovementsListQueryHandler : IRequestHandler<GetStockMovementsListQuery, PagedResult<StockMovementResponse>>
{
    private readonly IStockMovementRepository _stockMovementRepository;

    public GetStockMovementsListQueryHandler(IStockMovementRepository stockMovementRepository)
    {
        _stockMovementRepository = stockMovementRepository;
    }

    public async Task<PagedResult<StockMovementResponse>> Handle(
        GetStockMovementsListQuery request,
        CancellationToken cancellationToken)
    {
        var (movements, totalCount) = await _stockMovementRepository.GetPagedAsync(
            request.Filter,
            request.Pagination,
            cancellationToken);

        var items = movements.Select(m => new StockMovementResponse(
            m.Id,
            m.ProductId,
            m.WarehouseId,
            m.Type,
            m.Quantity,
            m.Notes,
            m.OccurredAtUtc,
            m.PerformedByUserId))
            .ToList();

        return new PagedResult<StockMovementResponse>(
            items,
            request.Pagination.Page,
            request.Pagination.PageSize,
            totalCount);
    }
}
