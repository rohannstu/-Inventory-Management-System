using InventoryManagementSystem.Api.Contracts.Products;
using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Products.Commands.CreateProduct;
using InventoryManagementSystem.Application.Products.Commands.DeleteProduct;
using InventoryManagementSystem.Application.Products.Commands.UpdateProduct;
using InventoryManagementSystem.Application.Products.Queries.GetProductById;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(
            Sku: request.Sku,
            Name: request.Name,
            Description: request.Description,
            Price: request.Price,
            Currency: request.Currency,
            CategoryId: request.CategoryId,
            SupplierId: request.SupplierId);

        var productId = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = productId }, new { id = productId });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(
            Id: id,
            Name: request.Name,
            Description: request.Description,
            Price: request.Price,
            Currency: request.Currency,
            CategoryId: request.CategoryId,
            SupplierId: request.SupplierId);

        var updated = await mediator.Send(command, cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await mediator.Send(new DeleteProductCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await mediator.Send(new GetProductByIdQuery(id), cancellationToken);

        return product is null ? NotFound() : Ok(product);
    }
}
