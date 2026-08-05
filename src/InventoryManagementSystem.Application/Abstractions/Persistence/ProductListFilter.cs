namespace InventoryManagementSystem.Application.Abstractions.Persistence;

public class ProductListFilter
{
    public Guid? CategoryId { get; set; }
    public Guid? SupplierId { get; set; }
    public bool? IsActive { get; set; }
    public string? SearchTerm { get; set; }
    public bool IncludeInactive { get; set; } = false;
}
