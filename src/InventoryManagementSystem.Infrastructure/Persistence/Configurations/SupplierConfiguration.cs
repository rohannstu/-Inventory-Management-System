using InventoryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Infrastructure.Persistence.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.ContactEmail).HasMaxLength(256);
        builder.Property(s => s.ContactPhone).HasMaxLength(32);
        builder.Property(s => s.IsActive).IsRequired();

        // Note: the reverse side of the Product <-> Supplier relationship
        // is already fully configured in ProductConfiguration via HasOne/WithMany.
        // We don't repeat it here — configuring it from one side is enough;
        // EF Core wires both directions from a single HasOne/WithMany pair.
    }
}