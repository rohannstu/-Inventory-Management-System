using InventoryManagementSystem.Domain.Common;

namespace InventoryManagementSystem.Domain.Entities;

public class Warehouse : Entity<Guid>
{
    public string Name { get; private set; } = default!;
    public string Location { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;

    private readonly List<StockMovement> _stockMovements = [];
    public IReadOnlyCollection<StockMovement> StockMovements => _stockMovements.AsReadOnly();

    private Warehouse() { } // EF Core

    public Warehouse(Guid id, string name, string location)
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Warehouse name is required.", nameof(name));

        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException("Warehouse location is required.", nameof(location));

        Name = name;
        Location = location;
        IsActive = true;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Warehouse name is required.", nameof(newName));

        Name = newName;
    }

    public void Relocate(string newLocation)
    {
        if (string.IsNullOrWhiteSpace(newLocation))
            throw new ArgumentException("Warehouse location is required.", nameof(newLocation));

        Location = newLocation;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}