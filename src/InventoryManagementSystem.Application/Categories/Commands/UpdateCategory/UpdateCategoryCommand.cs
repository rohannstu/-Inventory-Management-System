using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(
    Guid Id,
    string Name,
    string? Description) : ICommand<bool>;
