using InventoryManagementSystem.Api.Contracts.Categories;
using InventoryManagementSystem.Application.Abstractions.Messaging;
using InventoryManagementSystem.Application.Categories.Commands.CreateCategory;
using InventoryManagementSystem.Application.Categories.Commands.DeleteCategory;
using InventoryManagementSystem.Application.Categories.Commands.UpdateCategory;
using InventoryManagementSystem.Application.Categories.Queries.GetCategoryById;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var categoryId = await mediator.Send(new CreateCategoryCommand(
            Name: request.Name,
            Description: request.Description), cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = categoryId }, new { id = categoryId });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var updated = await mediator.Send(new UpdateCategoryCommand(
            Id: id,
            Name: request.Name,
            Description: request.Description), cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var category = await mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return category is null ? NotFound() : Ok(category);
    }
}
