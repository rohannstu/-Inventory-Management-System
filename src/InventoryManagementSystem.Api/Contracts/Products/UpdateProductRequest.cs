namespace InventoryManagementSystem.Api.Contracts.Products;

public record UpdateProductRequest(
    string Name,
    string? Description,
    decimal Price,
    string Currency,
    Guid CategoryId,
    Guid SupplierId);
