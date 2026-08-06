using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Suppliers.Commands.UpdateSupplier;

public sealed record UpdateSupplierCommand(
    Guid Id,
    string Name,
    string? ContactEmail,
    string? ContactPhone) : ICommand<bool>;
