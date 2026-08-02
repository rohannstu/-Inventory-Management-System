using InventoryManagementSystem.Domain.Common;

namespace InventoryManagementSystem.Domain.Entities;

public class Supplier : Entity<Guid>
{
    public string Name { get; private set; } = default!;
    public string? ContactEmail { get; private set; }
    public string? ContactPhone { get; private set; }
    public bool IsActive { get; private set; } = true;

    private readonly List<Product> _products = [];
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private Supplier() { } // EF Core

    public Supplier(Guid id, string name, string? contactEmail = null, string? contactPhone = null)
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Supplier name is required.", nameof(name));

        Name = name;
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        IsActive = true;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Supplier name is required.", nameof(newName));

        Name = newName;
    }

    public void UpdateContactInfo(string? email, string? phone)
    {
        ContactEmail = email;
        ContactPhone = phone;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}