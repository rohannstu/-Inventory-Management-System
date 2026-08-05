using InventoryManagementSystem.Api.Contracts.Products;
using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Abstractions.Pagination;
using InventoryManagementSystem.Application.Products.Commands.CreateProduct;
using InventoryManagementSystem.Application.Products.Commands.DeleteProduct;
using InventoryManagementSystem.Application.Products.Commands.UpdateProduct;
using InventoryManagementSystem.Application.Abstractions.Persistence;
using InventoryManagementSystem.Application.Products.Queries.GetProductById;
using InventoryManagementSystem.Application.Products.Queries.GetProductsList;
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

    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] ProductListFilter filter,
        [FromQuery] PaginationParams pagination,
        CancellationToken cancellationToken)
    {
        filter ??= new ProductListFilter();
        pagination ??= new PaginationParams();

        var result = await mediator.Send(new GetProductsListQuery(filter, pagination), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, [FromQuery] bool includeInactive, CancellationToken cancellationToken)
    {
        var product = await mediator.Send(new GetProductByIdQuery(id, includeInactive), cancellationToken);

        return product is null ? NotFound() : Ok(product);
    }
}
