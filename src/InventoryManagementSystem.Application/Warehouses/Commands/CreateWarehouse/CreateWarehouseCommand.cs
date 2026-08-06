using InventoryManagementSystem.Application.Abstractions.Messaging;

namespace InventoryManagementSystem.Application.Warehouses.Commands.CreateWarehouse;

public sealed record CreateWarehouseCommand(
    string Name,
    string Location) : ICommand<Guid>;
