using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Categories.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand(Guid Id) : ICommand<bool>;
