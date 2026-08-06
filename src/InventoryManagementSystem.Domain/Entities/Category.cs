using InventoryManagementSystem.Domain.Common;

namespace InventoryManagementSystem.Domain.Entities;

public class Category : Entity<Guid>
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    private Category() { } // required by EF Core later — do not delete

    public Category(Guid id, string name, string? description = null)
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.", nameof(name));

        Name = name;
        Description = description;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Category name is required.", nameof(newName));

        Name = newName;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }
}

