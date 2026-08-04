using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Sku,
    string Name,
    string? Description,
    decimal Price,
    string Currency,
    Guid CategoryId,
    Guid SupplierId
) : ICommand<Guid>;
