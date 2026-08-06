using InventoryManagementSystem.Api.Contracts.Warehouses;
using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Warehouses.Commands.CreateWarehouse;
using InventoryManagementSystem.Application.Warehouses.Commands.DeleteWarehouse;
using InventoryManagementSystem.Application.Warehouses.Commands.UpdateWarehouse;
using InventoryManagementSystem.Application.Warehouses.Queries.GetWarehouseById;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Api.Controllers;

[ApiController]
[Route("api/warehouses")]
public class WarehousesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateWarehouseRequest request, CancellationToken cancellationToken)
    {
        var warehouseId = await mediator.Send(new CreateWarehouseCommand(
            Name: request.Name,
            Location: request.Location), cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = warehouseId }, new { id = warehouseId });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateWarehouseRequest request, CancellationToken cancellationToken)
    {
        var updated = await mediator.Send(new UpdateWarehouseCommand(
            Id: id,
            Name: request.Name,
            Location: request.Location), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await mediator.Send(new DeleteWarehouseCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var warehouse = await mediator.Send(new GetWarehouseByIdQuery(id), cancellationToken);
        return warehouse is null ? NotFound() : Ok(warehouse);
    }
}
