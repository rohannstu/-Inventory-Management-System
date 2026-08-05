using InventoryManagementSystem.Domain.Entities;

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
    Guid SupplierId)
{
    public static ProductResponse FromEntity(Product product) => new(
        Id: product.Id,
        Sku: product.Sku.Value,
        Name: product.Name,
        Description: product.Description,
        Price: product.Price.Amount,
        Currency: product.Price.Currency,
        StockQuantity: product.StockQuantity,
        IsActive: product.IsActive,
        CategoryId: product.CategoryId,
        SupplierId: product.SupplierId);
}
