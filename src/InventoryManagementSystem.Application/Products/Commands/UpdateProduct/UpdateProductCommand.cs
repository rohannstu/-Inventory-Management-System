using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    string Currency,
    Guid CategoryId,
    Guid SupplierId
) : ICommand<bool>;
