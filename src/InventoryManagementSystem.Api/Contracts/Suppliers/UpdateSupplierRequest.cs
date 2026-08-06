namespace InventoryManagementSystem.Api.Contracts.Suppliers;

public sealed record UpdateSupplierRequest(
    string Name,
    string? ContactEmail,
    string? ContactPhone);
