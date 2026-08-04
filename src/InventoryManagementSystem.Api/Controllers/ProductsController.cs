using InventoryManagementSystem.Api.Contracts.Products;
using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Products.Commands.CreateProduct;
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

        return CreatedAtAction(nameof(Create), new { id = productId }, new { id = productId });
    }
}
