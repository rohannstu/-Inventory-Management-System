namespace InventoryManagementSystem.Application.Products.Queries.GetProductById;

public record ProductResponse(
    Guid Id,
    string Sku,
    string Name,
    string? Description,
    decimal Price,
    string Currency,
    int StockQuantity,
    bool IsActive,
    Guid CategoryId,
    Guid SupplierId);
