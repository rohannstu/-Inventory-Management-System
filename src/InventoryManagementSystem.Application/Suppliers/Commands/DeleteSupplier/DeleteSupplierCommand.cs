using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Suppliers.Commands.DeleteSupplier;

public sealed record DeleteSupplierCommand(Guid Id) : ICommand<bool>;
