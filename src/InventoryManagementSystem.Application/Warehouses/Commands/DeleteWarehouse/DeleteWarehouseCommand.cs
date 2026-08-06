using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Warehouses.Commands.DeleteWarehouse;

public sealed record DeleteWarehouseCommand(Guid Id) : ICommand<bool>;
