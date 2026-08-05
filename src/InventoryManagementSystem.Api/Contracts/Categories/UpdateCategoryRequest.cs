namespace InventoryManagementSystem.Api.Contracts.Categories;

public sealed record UpdateCategoryRequest(
    string Name,
    string? Description);
