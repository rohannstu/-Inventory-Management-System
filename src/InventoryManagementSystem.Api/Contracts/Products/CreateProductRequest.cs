namespace InventoryManagementSystem.Api.Contracts.Products;

public record CreateProductRequest(
    string Sku,
    string Name,
    string? Description,
    decimal Price,
    string Currency,
    Guid CategoryId,
    Guid SupplierId);
