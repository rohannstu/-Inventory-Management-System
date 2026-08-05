namespace InventoryManagementSystem.Api.Contracts.Warehouses;

public sealed record UpdateWarehouseRequest(
    string Name,
    string Location);
