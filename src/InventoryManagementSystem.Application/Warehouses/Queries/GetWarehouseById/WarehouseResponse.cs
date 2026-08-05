namespace InventoryManagementSystem.Application.Warehouses.Queries.GetWarehouseById;

public sealed record WarehouseResponse(
    Guid Id,
    string Name,
    string Location,
    bool IsActive);