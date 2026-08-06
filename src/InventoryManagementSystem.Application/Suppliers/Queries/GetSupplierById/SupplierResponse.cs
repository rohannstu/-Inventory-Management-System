namespace InventoryManagementSystem.Application.Suppliers.Queries.GetSupplierById;

public sealed record SupplierResponse(
    Guid Id,
    string Name,
    string? ContactEmail,
    string? ContactPhone,
    bool IsActive);
