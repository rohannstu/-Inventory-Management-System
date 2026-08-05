namespace InventoryManagementSystem.Application.Categories.Queries.GetCategoryById;

public sealed record CategoryResponse(
    Guid Id,
    string Name,
    string? Description);
