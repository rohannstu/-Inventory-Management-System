namespace InventoryManagementSystem.Api.Contracts.Suppliers;

public sealed record CreateSupplierRequest(
    string Name,
    string? ContactEmail,
    string? ContactPhone);
