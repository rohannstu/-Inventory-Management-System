namespace InventoryManagementSystem.Domain.ValueObjects;
//A Value Object is a small, immutable type defined entirely by its data — it has no identity,
//no Id, and two value objects with the same values
//are considered the same thing,full stop.Unlike an entity,
//you never ask "which one is this?" — you only ask "what does it equal?"

//Sku (a validated product identifier string)
public sealed record Sku
{
    public string Value { get; }

    private Sku(string value)
    {
        Value = value;
    }

    public static Sku Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("SKU cannot be empty.", nameof(value));

        var trimmed = value.Trim().ToUpperInvariant();

        if (trimmed.Length > 32)
            throw new ArgumentException("SKU cannot exceed 32 characters.", nameof(value));

        if (!trimmed.All(c => char.IsLetterOrDigit(c) || c == '-'))
            throw new ArgumentException("SKU can only contain letters, digits, and hyphens.", nameof(value));

        return new Sku(trimmed);
    }

    public override string ToString() => Value;
}