namespace InventoryManagementSystem.Api.Contracts.Warehouses;

public sealed record CreateWarehouseRequest(
    string Name,
    string Location);
