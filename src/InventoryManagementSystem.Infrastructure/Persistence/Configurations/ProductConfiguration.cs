using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        // Sku value object -> flattened onto the Products table as a single column,
        // via a value converter (Sku isn't a "real" table of its own).
        builder.Property(p => p.Sku)
            .HasConversion(
                sku => sku.Value,
                value => Sku.Create(value))
            .HasColumnName("Sku")
            .HasMaxLength(32)
            .IsRequired();

        builder.HasIndex(p => p.Sku).IsUnique();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        // Money value object -> flattened into TWO columns via OwnsOne,
        // since it has two fields (Amount, Currency), not one.
        builder.OwnsOne(p => p.Price, price =>
        {
            price.Property(m => m.Amount)
                .HasColumnName("Price_Amount")
                .HasColumnType("numeric(18,2)")
                .IsRequired();

            price.Property(m => m.Currency)
                .HasColumnName("Price_Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(p => p.StockQuantity).IsRequired();
        builder.Property(p => p.IsActive).IsRequired();

        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}