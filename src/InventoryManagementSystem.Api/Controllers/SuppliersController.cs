using InventoryManagementSystem.Api.Contracts.Suppliers;
using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Suppliers.Commands.CreateSupplier;
using InventoryManagementSystem.Application.Suppliers.Commands.DeleteSupplier;
using InventoryManagementSystem.Application.Suppliers.Commands.UpdateSupplier;
using InventoryManagementSystem.Application.Suppliers.Queries.GetSupplierById;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Api.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateSupplierRequest request, CancellationToken cancellationToken)
    {
        var supplierId = await mediator.Send(new CreateSupplierCommand(
            Name: request.Name,
            ContactEmail: request.ContactEmail,
            ContactPhone: request.ContactPhone), cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = supplierId }, new { id = supplierId });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateSupplierRequest request, CancellationToken cancellationToken)
    {
        var updated = await mediator.Send(new UpdateSupplierCommand(
            Id: id,
            Name: request.Name,
            ContactEmail: request.ContactEmail,
            ContactPhone: request.ContactPhone), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await mediator.Send(new DeleteSupplierCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var supplier = await mediator.Send(new GetSupplierByIdQuery(id), cancellationToken);
        return supplier is null ? NotFound() : Ok(supplier);
    }
}
