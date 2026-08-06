using InventoryManagementSystem.Api.Contracts.StockMovements;
using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Pagination;
using InventoryManagementSystem.Application.StockMovements.Commands.CreateStockMovement;
using InventoryManagementSystem.Application.StockMovements.Commands.DeleteStockMovement;
using InventoryManagementSystem.Application.StockMovements.Queries.GetStockMovementById;
using InventoryManagementSystem.Application.StockMovements.Queries.GetStockMovementsList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Api.Controllers;

[ApiController]
[Route("api/stockmovements")]
[Authorize(Policy = "RequireAnyRole")]
public class StockMovementsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "RequireManagerOrAbove")]
    public async Task<IActionResult> Create(CreateStockMovementRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateStockMovementCommand(
            ProductId: request.ProductId,
            WarehouseId: request.WarehouseId,
            Type: request.Type,
            Quantity: request.Quantity,
            PerformedByUserId: request.PerformedByUserId,
            Notes: request.Notes);

        var stockMovementId = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = stockMovementId }, new { id = stockMovementId });
    }

    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] StockMovementListFilter filter,
        [FromQuery] PaginationParams pagination,
        CancellationToken cancellationToken)
    {
        filter ??= new StockMovementListFilter();
        pagination ??= new PaginationParams();

        var result = await mediator.Send(new GetStockMovementsListQuery(filter, pagination), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await mediator.Send(new DeleteStockMovementCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var stockMovement = await mediator.Send(new GetStockMovementByIdQuery(id), cancellationToken);
        return stockMovement is null ? NotFound() : Ok(stockMovement);
    }
}
