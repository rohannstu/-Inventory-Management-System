using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<bool>;
