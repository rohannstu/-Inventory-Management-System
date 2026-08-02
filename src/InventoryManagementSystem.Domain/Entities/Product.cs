using InventoryManagementSystem.Domain.Common;
using InventoryManagementSystem.Domain.ValueObjects;

namespace InventoryManagementSystem.Domain.Entities;

public class Product : Entity<Guid>
{
    public Sku Sku { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public Money Price { get; private set; } = default!;
    public int StockQuantity { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Guid CategoryId { get; private set; }
    public Category? Category { get; private set; } // navigation — Category already exists

    public Guid SupplierId { get; private set; }
    // No Supplier navigation property yet — that entity doesn't exist.
    // We'll add "public Supplier? Supplier { get; private set; }" once it does.

    private Product() { } // EF Core

    public Product(
        Guid id,
        Sku sku,
        string name,
        Money price,
        Guid categoryId,
        Guid supplierId,
        string? description = null)
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required.", nameof(name));

        Sku = sku ?? throw new ArgumentNullException(nameof(sku));
        Name = name;
        Price = price ?? throw new ArgumentNullException(nameof(price));
        CategoryId = categoryId;
        SupplierId = supplierId;
        Description = description;
        StockQuantity = 0;
        IsActive = true;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Product name is required.", nameof(newName));

        Name = newName;
    }

    public void ChangePrice(Money newPrice)
    {
        Price = newPrice ?? throw new ArgumentNullException(nameof(newPrice));
    }

    public void ChangeCategory(Guid categoryId)
    {
        CategoryId = categoryId;
    }

    public void ChangeSupplier(Guid supplierId)
    {
        SupplierId = supplierId;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to increase must be positive.", nameof(quantity));

        StockQuantity += quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity to decrease must be positive.", nameof(quantity));

        if (quantity > StockQuantity)
            throw new InvalidOperationException(
                $"Cannot decrease stock by {quantity}; only {StockQuantity} available.");

        StockQuantity -= quantity;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}