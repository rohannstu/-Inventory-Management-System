using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Warehouses.Commands.UpdateWarehouse;

public sealed record UpdateWarehouseCommand(
    Guid Id,
    string Name,
    string Location) : ICommand<bool>;
